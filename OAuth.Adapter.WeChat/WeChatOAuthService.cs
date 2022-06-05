using Microsoft.Extensions.Options;
using OAuth.Adapter.WeChat.Options;
using OAuth.Adapter.WeChat.Requests;
using System.Threading.Tasks;

namespace OAuth.Adapter.WeChat
{
    public class WeChatOAuthService : IWeChatOAuthService
    {
        private readonly IWeChatAuthService _weChatAuthService;
        private readonly WeChatOptions _weChatOptions;

        public WeChatOAuthService(IWeChatAuthService weChatAuthService,
                                  IOptions<WeChatOptions> weChatOptions)
        {
            _weChatAuthService = weChatAuthService;
            _weChatOptions = weChatOptions.Value;
        }

        public async Task<WeChatAuth> GetUserSessionAsync(string code)
        {
            var session = await _weChatAuthService.JsCodeToSessionAsync(new CodeToSessionRequest
            {
                AppId = _weChatOptions.AppId,
                Code = code,
                Secret = _weChatOptions.Secret
            });

            if (!session.IsSuccess)
            {
                return null;
            }

            return new WeChatAuth
            {
                SessionKey = session.SessionKey,
                OpenId = session.OpenId
            };
        }
    }
}
