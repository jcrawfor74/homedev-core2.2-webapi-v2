using Newtonsoft.Json;

namespace HomeDev.Core.WebApi.Models
{
    [JsonObject("tokenSettings")]
    public class TokenSettings
    {
        [JsonProperty("secret")]
        public string Secret { get; set; }

        [JsonProperty("issuer")]
        public string Issuer { get; set; }

        [JsonProperty("audience")]
        public string Audience { get; set; }

        [JsonProperty("accessExpiration")]
        public long AccessExpiration { get; set; }

        [JsonProperty("refreshExpiration")]
        public long RefreshExpiration { get; set; }
    }
}