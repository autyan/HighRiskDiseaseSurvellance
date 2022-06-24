using aspnetapp.TokenProvider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OAuth.Adapter.WeChat;
using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Aplication.Services;
using HighRiskDiseaseSurvellance.Dto;
using HighRiskDiseaseSurvellance.Dto.Requests;
using HighRiskDiseaseSurvellance.Infrastructure;
using NuGet.Protocol;

namespace aspnetapp.Controllers.Api
{
    public class AuthController : ApiController
    {
        private readonly IWeChatOAuthService _weChatOAuthService;
        private readonly UserService         _userService;
        private readonly ITokenProvider      _tokenProvider;
        private readonly ILogger             _logger;

        public AuthController(IWeChatOAuthService     weChatOAuthService,
                              UserService             userService,
                              ILogger<AuthController> logger, 
                              ITokenProvider          tokenProvider)
        {
            _weChatOAuthService = weChatOAuthService;
            _userService        = userService;
            _logger             = logger;
            _tokenProvider      = tokenProvider;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<AuthToken> WeChat([FromBody] WeChatAuthRequest request)
        {
            var wechatUserSession = await _weChatOAuthService.GetUserSessionAsync(request.Code).ConfigureAwait(false);
            if (wechatUserSession == null)
            {
                _logger.LogError($"wechat session fetch failed, request:{request.ToJson()}");
                throw new DomainException(ErrorCode.WeChatUserLoginFailed);
            }
            var user = await _userService.UserLoginAsync(new UserLoginRequest
                                                         {
                                                             WeChatOpenId = wechatUserSession.OpenId,
                                                             DistributorId = request.DistributorId,
                                                         });
            if (user == null)
            {
                _logger.LogError($"user not found, request:{request.ToJson()}");
                throw new DomainException(ErrorCode.UserNotFound);
            }

            var token = _tokenProvider.GenerateToken(new SignInUser
                                                     {
                                                         Id               = user.Id,
                                                         NickName         = user.NickName,
                                                         PhoneNumber      = user.PhoneNumber,
                                                         WeChatOpenId     = wechatUserSession.OpenId,
                                                         WeChatSessionKey = wechatUserSession.SessionKey
                                                     });

            token.UserInfo = user;
            return token;
        }
    }
}
