using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using RDStation.Interfaces;
using RDStation.Entities.Common.Enums;
using System.ComponentModel;
using System.Net;

namespace RDStation.Entities
{
    /// <summary>
    /// Base abstract class for responses.
    /// </summary>
    public abstract class BaseResponse : IResponse
    {
        /// <summary>
        /// See <see cref="IResponse.RawJson"/>.
        /// </summary>
        public virtual string RawJson { get; set; }

        /// <summary>
        /// See <see cref="IResponse.RawQueryString"/>.
        /// </summary>
        public virtual string RawQueryString { get; set; }

        /// <summary>
        /// See <see cref="IResponse.Status"/>.
        /// </summary>
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public virtual HttpStatusCode? Status { get; set; }

        /// <summary>
        /// See <see cref="IResponse.ErrorMessage"/>.
        /// </summary>
        [JsonProperty("error_message")]
        public virtual string? ErrorMessage { get; set; }

        /// <summary>
        /// See <see cref="IResponse.ErrorType"/>.
        /// </summary>
        [JsonProperty("error_type")]
        public virtual string? ErrorType { get; set; }

        /// <summary>
        /// See <see cref="IResponse.HtmlAttributions"/>.
        /// </summary>
        [JsonProperty("html_attributions")]
        public virtual IEnumerable<string> HtmlAttributions { get; set; }

        public virtual Error? error { get; set; }

        public class Error
        {
            /// <summary>
            /// See <see cref="IResponse.ErrorMessage"/>.
            /// </summary>
            [JsonProperty("error_message")]
            public virtual string? ErrorMessage { get; set; }

            /// <summary>
            /// See <see cref="IResponse.ErrorType"/>.
            /// </summary>
            [JsonProperty("error_type")]
            public virtual string? ErrorType { get; set; }
        }
    }
}
