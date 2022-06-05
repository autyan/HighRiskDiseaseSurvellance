using HighRiskDiseaseSurvellance.Aplication.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HighRiskDiseaseSurvellance.Aplication
{
    public static class Extensions
    {
        public static IServiceCollection UserRedPacketService(this IServiceCollection service)
        {
            service.AddScoped<UserService>();
            service.AddScoped<RecordService>();
            service.AddScoped<OfficeUserService>();
            return service;
        }
    }
}
