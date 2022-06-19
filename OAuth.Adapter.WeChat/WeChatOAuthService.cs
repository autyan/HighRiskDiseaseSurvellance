using System;
using Microsoft.Extensions.Options;
using OAuth.Adapter.WeChat.Options;
using OAuth.Adapter.WeChat.Requests;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using OAuth.Adapter.WeChat.Models;

namespace OAuth.Adapter.WeChat
{
    public class WeChatOAuthService : IWeChatOAuthService
    {
        private readonly IWeChatAuthService           _weChatAuthService;
        private readonly WeChatOptions                _weChatOptions;
        private          GetWeChatAccessTokenResponse _accessToken;
        private          DateTime                     _accessTokenExpireTime;
        private readonly IHostingEnvironment          _hostingEnvironment;

        public WeChatOAuthService(IWeChatAuthService      weChatAuthService,
                                  IOptions<WeChatOptions> weChatOptions, 
                                  IHostingEnvironment     hostingEnvironment)
        {
            _weChatAuthService  = weChatAuthService;
            _hostingEnvironment = hostingEnvironment;
            _weChatOptions      = weChatOptions.Value;
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

        public async Task<byte[]> GetUnlimitedCodeAsync(string scene)
        {
            var accessToken = await GetAccessTokenAsync();
            var responseMessage =  await _weChatAuthService.GetUnlimitedCodeAsync(accessToken.AccessToken, new UnlimitedCodeRequest
                                                                                                           {
                                                                                                               Scene = scene,
                                                                                                               EnvironmentVersion = GetEnvironmentVersion()
                                                                                                           });
            return await responseMessage.Content.ReadAsByteArrayAsync();
        }

        private async Task<GetWeChatAccessTokenResponse> GetAccessTokenAsync()
        {
            if (_accessToken == null || DateTime.Now > _accessTokenExpireTime)
            {
                GetWeChatAccessTokenResponse newToken = null;
                while (newToken == null || newToken.ErrorCode != 0)
                {
                    newToken = await _weChatAuthService.GetAccessTokenAsync("client_credential",
                                                                            _weChatOptions.AppId,
                                                                            _weChatOptions.Secret);
                }

                _accessToken           = newToken;
                _accessTokenExpireTime = DateTime.Now.AddMinutes(-5).AddSeconds(newToken.ExpiresIn);
            }

            return _accessToken;
        }

        private string GetEnvironmentVersion()
        {
            if (_hostingEnvironment.IsDevelopment()) return "develop";
            if (_hostingEnvironment.IsStaging()) return "trial";
            return "release";
        }
    }
}
