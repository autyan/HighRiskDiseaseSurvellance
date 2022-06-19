using System.Text.Json.Serialization;

namespace OAuth.Adapter.WeChat.Models
{
    public class GetWeChatAccessTokenResponse : ErrorResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
    }
}
