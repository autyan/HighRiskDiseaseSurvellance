using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace OAuth.Adapter.WeChat.Requests
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddWeChatAuth(this IServiceCollection services)
        {
            var wechatAuthService = RestService.For<IWeChatAuthService>("https://api.weixin.qq.com");
            services.AddSingleton(wechatAuthService);
            services.AddScoped<IWeChatOAuthService, WeChatOAuthService>();
            return services;
        }
    }
}
