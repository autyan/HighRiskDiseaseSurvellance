using System.Text.Json.Serialization;

namespace OAuth.Adapter.WeChat.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("errcode")]
        public int ErrorCode { get; set; }

        public string ErrMsg { get; set; }

        public bool IsSuccess => ErrorCode == 0;
    }
}
