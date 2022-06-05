using System.Threading.Tasks;

namespace OAuth.Adapter.WeChat
{
    public interface IWeChatOAuthService
    {
        Task<WeChatAuth> GetUserSessionAsync(string code);
    }
}
