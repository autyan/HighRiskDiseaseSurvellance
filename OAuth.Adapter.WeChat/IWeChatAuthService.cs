using System.Net.Http;
using OAuth.Adapter.WeChat.Models;
using OAuth.Adapter.WeChat.Requests;
using Refit;
using System.Threading.Tasks;

namespace OAuth.Adapter.WeChat
{
    public interface IWeChatAuthService
    {
        [Get("/sns/jscode2session")]
        Task<JsCodeToSessionResponse> JsCodeToSessionAsync(CodeToSessionRequest request);
        
        [Get("/cgi-bin/token")]
        Task<GetWeChatAccessTokenResponse> GetAccessTokenAsync([AliasAs("grant_type")]string grantType, string appid, string secret);

        [Post("/wxa/getwxacodeunlimit?access_token={accessToken}")]
        Task<HttpResponseMessage> GetUnlimitedCodeAsync(string accessToken, [Body(BodySerializationMethod.Serialized, true)]UnlimitedCodeRequest request);

        [Post("/wxa/getwxacodeunlimit")]
        Task<HttpResponseMessage> GetUnlimitedCodeNoAccessTokenAsync([Body(BodySerializationMethod.Serialized, true)]UnlimitedCodeRequest request);
    }
}