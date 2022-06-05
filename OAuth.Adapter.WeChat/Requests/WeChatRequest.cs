using Refit;

namespace OAuth.Adapter.WeChat.Requests
{
    public class WeChatRequest
    {
        [AliasAs("access_token")]
        public string AccessToken { get; set; }

        [AliasAs("openid")]
        public string OpenId { get; set; }
    }
}
