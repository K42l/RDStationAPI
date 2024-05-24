using System.Net.Http.Headers;
using System.Net;
using System.Text;
using RDStation.Interfaces;
using RDStation.Exceptions;
using Newtonsoft.Json;
using RDStation.Entities;
using System.Runtime.InteropServices.Marshalling;

namespace RDStation
{
    public abstract class HttpEngine : IDisposable
    {
        private static HttpClient httpClient;
        private static readonly TimeSpan httpTimeout = new TimeSpan(0, 0, 30);
        private static Credentials credentials { get; set; }
        /// <summary>
        /// Http Client.
        /// </summary>
        protected internal static HttpClient HttpClient
        {
            get
            {
                if (HttpEngine.httpClient == null)
                {
                    //Setup();
                    var httpClientHandler = new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                        UseCookies = false
                    };

                    if (HttpEngine.Proxy != null)
                        httpClientHandler.Proxy = HttpEngine.Proxy;

                    HttpEngine.httpClient = new HttpClient(httpClientHandler)
                    {
                        Timeout = HttpEngine.httpTimeout
                    };

                    //Yeah i know this is ugly and i don't like it either, but i was in a hurry and had to make it functional as quick as possible
                    credentials = JsonConvert.DeserializeObject<Credentials>(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "credentials.json"));

                    HttpEngine.httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpEngine.HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credentials.RDStationCredentials.access_token);
                }
                return HttpEngine.httpClient;
            }
            set => HttpEngine.httpClient = value;
        }

        /// <summary>
        /// Proxy property that will be used for all requests.
        /// </summary>
        public static IWebProxy Proxy { get; set; }

        /// <summary>
        /// Disposes.
        /// </summary>
        public virtual void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the <see cref="HttpClient"/>, if <paramref name="disposing"/> is true.
        /// </summary>
        /// <param name="disposing">Whether to dispose resources or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
                return;

            HttpEngine.HttpClient?.Dispose();
            HttpEngine.httpClient = null;
        }
    }
    /// <summary>
    /// Http Engine.
    /// Manges the http connections, and is responsible for invoking requst and handling responses.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public sealed class HttpEngine<TRequest, TResponse> : HttpEngine
        where TRequest : IRequest, new()
        where TResponse : IResponse, new()
    {
        internal static readonly HttpEngine<TRequest, TResponse> instance = new();

        /// <summary>
        /// Query.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="httpEngineOptions">The <see cref="HttpEngineOptions"/>.</param>
        /// <returns>The <see cref="IResponse"/>.</returns>
        public TResponse Query(TRequest request, HttpEngineOptions? httpEngineOptions = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            httpEngineOptions = httpEngineOptions ?? new HttpEngineOptions();
            try
            {
                using (var result = this.ProcessRequest(request))
                {
                    var response = this.ProcessResponse(result);
                    
                    switch (response.Status)
                    {
                        case HttpStatusCode.OK:
                        case HttpStatusCode.NotFound:
                        case HttpStatusCode.Unauthorized:
                        case HttpStatusCode.TooManyRequests:
                            return response;
                        case HttpStatusCode.BadRequest:
                        case HttpStatusCode.UnsupportedMediaType:
                        case HttpStatusCode.Forbidden:
                        case HttpStatusCode.UnprocessableEntity:
                        case HttpStatusCode.InternalServerError:
                        default:
                            if (!httpEngineOptions.ThrowOnInvalidRequest)
                            {
                                return response;
                            }
                            throw new RDStationExceptions($"{response.Status}: {response.ErrorMessage ?? "No message"}");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is RDStationExceptions)
                    throw;

                throw new RDStationExceptions(ex.Message);
            }
        }

        /// <summary>
        /// Query Async.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<TResponse> QueryAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var taskCompletion = new TaskCompletionSource<TResponse>();
            
            await this.ProcessRequestAsync(request, cancellationToken)
                .ContinueWith(async x =>
                {
                    using (x)
                    {
                        try
                        {
                            if (x.IsCanceled)
                            {
                                taskCompletion.SetCanceled();
                            }
                            else if (x.IsFaulted)
                            {
                                throw x.Exception ?? new Exception();
                            }
                            else
                            {
                                using (var result = await x)
                                {
                                    var response = await this.ProcessResponseAsync(result).ConfigureAwait(false);

                                    x.Dispose();
                                    x = null;

                                    switch (response.Status)
                                    {
                                        case HttpStatusCode.OK:
                                        case HttpStatusCode.NotFound:
                                        case HttpStatusCode.Unauthorized:
                                        case HttpStatusCode.TooManyRequests:
                                            taskCompletion.SetResult(response);
                                            break;
                                        case HttpStatusCode.BadRequest:
                                        case HttpStatusCode.UnsupportedMediaType:
                                        case HttpStatusCode.Forbidden:
                                        case HttpStatusCode.UnprocessableEntity:
                                        case HttpStatusCode.InternalServerError:
                                        default:
                                            throw new RDStationExceptions($"{response.Status}: {response.ErrorMessage}");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex is RDStationExceptions)
                            {
                                taskCompletion.SetException(ex);
                            }
                            else
                            {
                                var baseException = ex.GetBaseException();
                                var exception = new RDStationExceptions(baseException.Message, baseException);

                                taskCompletion.SetException(exception);
                            }
                        }
                    }  
                }, cancellationToken)
                .ConfigureAwait(false);

            return await taskCompletion.Task;
        }

        /// <summary>
        /// Query Async.
        /// </summary>
        /// <param name="request">The request that will be sent.</param>
        /// <param name="httpEngineOptions">The <see cref="HttpEngineOptions"/></param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
        /// <returns>The <see cref="Task{T}"/>.</returns>
        public async Task<TResponse> QueryAsync(TRequest request, HttpEngineOptions httpEngineOptions, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            httpEngineOptions = httpEngineOptions ?? new HttpEngineOptions();

            var taskCompletion = new TaskCompletionSource<TResponse>();
            
            await this.ProcessRequestAsync(request, cancellationToken)
                .ContinueWith(async x =>
                {
                    using (x)
                    {
                        try
                        {
                            if (x.IsCanceled)
                            {
                                taskCompletion.SetCanceled();
                            }
                            else if (x.IsFaulted)
                            {
                                throw x.Exception ?? new Exception();
                            }
                            else
                            {
                                using (var result = await x)
                                {
                                    var response = await this.ProcessResponseAsync(result).ConfigureAwait(false);

                                    switch (response.Status)
                                    {
                                        case HttpStatusCode.OK:
                                        case HttpStatusCode.NotFound:
                                        case HttpStatusCode.Unauthorized:
                                        case HttpStatusCode.TooManyRequests:
                                            taskCompletion.SetResult(response);
                                            break;
                                        case HttpStatusCode.BadRequest:
                                        case HttpStatusCode.UnsupportedMediaType:
                                        case HttpStatusCode.Forbidden:
                                        case HttpStatusCode.UnprocessableEntity:
                                        case HttpStatusCode.InternalServerError:
                                        default:
                                            if (!httpEngineOptions.ThrowOnInvalidRequest)
                                            {
                                                taskCompletion.SetResult(response);
                                                break;
                                            }
                                            throw new RDStationExceptions($"{response.Status}: {response.ErrorMessage ?? "No message"}");
                                    }
                                    taskCompletion.SetResult(response);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (ex is RDStationExceptions)
                            {
                                taskCompletion.SetException(ex);
                            }
                            else
                            {
                                var baseException = ex.GetBaseException();
                                var exception = new RDStationExceptions(baseException.Message, baseException);

                                taskCompletion.SetException(exception);
                            }
                        }
                    }
                }, cancellationToken)
                .ConfigureAwait(false);
            return await taskCompletion.Task;
        }
        private HttpResponseMessage ProcessRequest(TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = request.GetUri();

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            
            var serializeObject = "";
            if (request.DataJsonPost is not null)
            {
                serializeObject = JsonConvert.SerializeObject(request.DataJsonPost, settings);
            }
            else
            {
                serializeObject = JsonConvert.SerializeObject(request, settings);
            }

            StringContent? httpContent;

            if (uri.ToString() == "https://api.rd.services/auth/token?")
                HttpEngine.HttpClient.DefaultRequestHeaders.Authorization = null;

            switch (request)
            {
                case (IRequestQueryString):
                    return HttpEngine.HttpClient.GetAsync(uri).Result;
                case (IRequestJsonPost):
                    using (httpContent = new StringContent(serializeObject, Encoding.UTF8, "application/json"))
                    {
                        httpContent.Headers.ContentType.CharSet = string.Empty;
                        return HttpEngine.HttpClient.PostAsync(uri, httpContent).Result;
                    }
                case (IRequestJsonPut):
                    using (httpContent = new StringContent(serializeObject, Encoding.UTF8, "application/json"))
                    {
                        httpContent.Headers.ContentType.CharSet = string.Empty;
                        return HttpEngine.HttpClient.PutAsync(uri, httpContent).Result;
                    }
                case (IRequestJsonPatch):
                    using (httpContent = new StringContent(serializeObject, Encoding.UTF8, "application/json"))
                    {
                        httpContent.Headers.ContentType.CharSet = string.Empty;
                        return HttpEngine.HttpClient.PatchAsync(uri, httpContent).Result;
                    }
                case (IRequestJsonDelete):
                    using (httpContent = new StringContent(serializeObject, Encoding.UTF8, "application/json"))
                    {
                        httpContent.Headers.ContentType.CharSet = string.Empty;
                        return HttpEngine.HttpClient.DeleteAsync(uri).Result;
                    }                    
                default:
                    return HttpEngine.HttpClient.GetAsync(uri).Result;
            }
        }

        private async Task<HttpResponseMessage> ProcessRequestAsync(TRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var uri = request.GetUri();

            if (request is IRequestQueryString)
                return await HttpEngine.HttpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };
            string serializeObject = JsonConvert.SerializeObject(request, settings);

            using (var stringContent = new StringContent(serializeObject, Encoding.UTF8))
            {
                using (var content = await stringContent.ReadAsStreamAsync().ConfigureAwait(false))
                {
                    using (var streamContent = new StreamContent(content))
                    {
                        return await HttpEngine.HttpClient.PostAsync(uri, streamContent, cancellationToken).ConfigureAwait(false);
                    }
                }
            }
        }
        private TResponse ProcessResponse(HttpResponseMessage httpResponse)
        {
            if (httpResponse == null)
                throw new ArgumentNullException(nameof(httpResponse));

            using (httpResponse)
            {
                var response = new TResponse();

                switch (response)
                {
                    case BaseResponseStream streamResponse:
                        streamResponse.Buffer = httpResponse.Content.ReadAsByteArrayAsync().Result;
                        response = (TResponse)(IResponse)streamResponse;
                        break;

                    default:
                        var rawJson = httpResponse.Content.ReadAsStringAsync().Result;

                        response = JsonConvert.DeserializeObject<TResponse>(rawJson);

                        if (response == null)
                            throw new NullReferenceException(nameof(response));

                        response.RawJson = rawJson;
                        break;
                }

                response.RawQueryString = httpResponse.RequestMessage.RequestUri.PathAndQuery;
                response.Status = httpResponse.StatusCode;

                return response;
            }
        }
        private async Task<TResponse> ProcessResponseAsync(HttpResponseMessage httpResponse)
        {
            if (httpResponse == null)
                throw new ArgumentNullException(nameof(httpResponse));

            using (httpResponse)
            {

                var response = new TResponse();

                switch (response)
                {
                    case BaseResponseStream streamResponse:
                        streamResponse.Buffer = await httpResponse.Content.ReadAsByteArrayAsync();
                        response = (TResponse)(IResponse)streamResponse;

                        break;

                    default:
                        var rawJson = await httpResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        
                        

                        response = JsonConvert.DeserializeObject<TResponse>(rawJson);

                        if (response == null)
                            throw new NullReferenceException(nameof(response));

                        response.RawJson = rawJson;
                        break;
                }

                response.RawQueryString = httpResponse.RequestMessage.RequestUri.PathAndQuery;
                response.Status = httpResponse.StatusCode;

                return response;
            }
        }
    }
}
