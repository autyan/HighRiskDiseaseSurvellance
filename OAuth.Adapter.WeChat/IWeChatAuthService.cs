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
    }
}