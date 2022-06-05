using Refit;

namespace OAuth.Adapter.WeChat.Requests
{
    public class CodeToSessionRequest
    {
        public string AppId { get; set; }

        public string Secret { get; set; }

        [AliasAs("js_code")]
        public string Code { get; set; }

        [AliasAs("grant_type")]
        public string GrantType { get; set; } = "authorization_code";
    }
}
