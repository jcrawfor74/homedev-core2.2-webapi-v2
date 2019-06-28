using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace HomeDev.Core.WebApi.Models
{
    public class AuthTokenRequest
    {
        [Required]
        [JsonProperty("username")]
        public string Username { get; set; }

        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}