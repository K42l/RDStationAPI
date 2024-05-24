using RDStation.Interfaces;
using System.Text.Json.Serialization;

namespace RDStation.Entities
{
    public class BaseRequest: IRequest
    {
        /// <summary>
        /// See <see cref="IRequest.BaseUrl"/>.
        /// </summary>
        [JsonIgnore]
        public virtual string BaseUrl { get; set; }
        [JsonIgnore]
        public virtual object DataJsonPost { get; set; }
        [JsonIgnore]
        public string queryStringParametersTemp { get; set; }

        /// <summary>
        /// See <see cref="IRequest.GetUri()"/>.
        /// </summary>
        /// <returns>The <see cref="Uri"/>.</returns>
        public virtual Uri GetUri()
        {
            const string SCHEME = "https://";
            string BaseUrlContext = BaseUrl;
            var getQueryStringWithoutParameters = this.GetQueryStringWithoutParameters();

            foreach (var item in getQueryStringWithoutParameters)
            {
                var param = "{" + item.Key + "}";
                BaseUrlContext = BaseUrlContext.Replace(param, item.Value);
            }

            foreach (var item in this.GetQueryStringParameters())
            {
                if (item.Key == "data")
                    this.queryStringParametersTemp = item.Value;
            }

            var queryStringWithoutParameters = getQueryStringWithoutParameters
                .Select(x =>
                    x.Value == null
                        ? Uri.EscapeDataString(x.Key)
                        : Uri.EscapeDataString(x.Value));
            var queryWithoutString = string.Join("/", queryStringWithoutParameters);


            var queryStringParameters = this.GetQueryStringParameters()
                .Select(x =>
                    x.Value == null
                        ? Uri.EscapeDataString(x.Key)
                        : Uri.EscapeDataString(x.Key) + "=" + Uri.EscapeDataString(x.Value));
            var queryString = string.Join("&", queryStringParameters);

            Uri uri;

            if (String.IsNullOrEmpty(queryString))
            {
                uri = new Uri($"{SCHEME}api.rd.services/{BaseUrlContext}");
            }
            else
            {
                uri = new Uri($"{SCHEME}api.rd.services/{BaseUrlContext}?{queryString}");
            }

            var url = $"{uri.LocalPath}{uri.Query}";

            return new Uri($"{uri.Scheme}://{uri.Host}{url}");
        }


        /// <summary>
        /// See <see cref="IRequest.GetQueryStringParameters()"/>.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair}"/> collection.</returns>
        public virtual IList<KeyValuePair<string, string>> GetQueryStringParameters()
        {
            var parameters = new List<KeyValuePair<string, string>>();
            return parameters;
        }

        /// <summary>
        /// See <see cref="IRequest.GetQueryStringParameters()"/>.
        /// </summary>
        /// <returns>The <see cref="IList{KeyValuePair}"/> collection.</returns>
        public virtual IList<KeyValuePair<string, string>> GetQueryStringWithoutParameters()
        {
            var parameters = new List<KeyValuePair<string, string>>();
            return parameters;
        }

    }
}
