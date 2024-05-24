namespace RDStation.Entities.Authentication.ObtainTokens.Response
{
    public class ObtainTokensResponse : BaseResponse
    {
        /// <summary>
        /// Token used on the authentication
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// Time to token expire in seconds (Default: 86400)
        /// </summary>
        public string expires_in { get; set; }
        
        /// <summary>
        /// refresh_token returned upon token creation
        /// </summary>
        public string refresh_token { get; set; }
    }
}
