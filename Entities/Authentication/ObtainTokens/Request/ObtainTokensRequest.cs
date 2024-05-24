namespace RDStation.Entities.Authentication.ObtainTokens.Request
{
    public class ObtainTokensRequest : BaseRequest
    {
        public override string BaseUrl => "auth/token?token_by=code";

        /// <summary>
        /// client_id of the application
        /// </summary>
        public string client_id { get; set; }

        /// <summary>
        /// client_secret of the application
        /// </summary>
        public string client_secret { get; set;}

        /// <summary>
        /// code received in the callback URL. (The code is valid for 1 hour)
        /// </summary>
        public string code { get; set; }

    }
}
