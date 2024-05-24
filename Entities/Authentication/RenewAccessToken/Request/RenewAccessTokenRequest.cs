using RDStation.Interfaces;

namespace RDStation.Entities.Authentication.RenewAccessToken.Request
{
    public class RenewAccessTokenRequest : BaseRequest, IRequest
    {
        public override string BaseUrl => "auth/token";

        /// <summary>
        /// client_id of the application
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// client_secret of the application
        /// </summary>
        public string client_secret { get; set; }

        /// <summary>
        /// refresh_token returned upon token creation
        /// </summary>
        public string refresh_token { get; set; }

    }
}
