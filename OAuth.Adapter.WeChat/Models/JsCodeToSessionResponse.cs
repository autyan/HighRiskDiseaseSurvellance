using System.Text.Json.Serialization;

namespace OAuth.Adapter.WeChat.Models
{
    public class JsCodeToSessionResponse : ErrorResponse
    {
        public string OpenId { get; set; }

        [JsonPropertyName("session_key")]
        public string SessionKey { get; set; }

    }
}
