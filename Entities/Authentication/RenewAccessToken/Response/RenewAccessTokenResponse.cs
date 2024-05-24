namespace RDStation.Entities.Authentication.RenewAccessToken.Response
{
    public class RenewAccessTokenResponse : BaseResponse
    {
        /// <summary>
        /// access_token is used to authorize your requests. It has a predetermined expiration date defined by the attribute expires_in seconds, which is 86400 seconds (24 Hours)
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
