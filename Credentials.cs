using Newtonsoft.Json;

namespace RDStation
{
    public class Credentials
    {
        [JsonProperty("RDStationCredentials")]
        public RdStationCredentials RDStationCredentials { get; set; }
        public class RdStationCredentials
        {
            [JsonProperty("access_token")]
            public string access_token { get; set; }

            [JsonProperty("refresh_token")]
            public string refresh_token { get; set; }

            [JsonProperty("client_id")]
            public string client_id { get; set; }

            [JsonProperty("client_secret")]
            public string client_secret { get; set; }
        }
    }
}
