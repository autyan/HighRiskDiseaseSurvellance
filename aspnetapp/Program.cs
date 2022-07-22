using aspnetapp.Filters;
using aspnetapp.TokenProvider;
using HighRiskDiseaseSurvellance.Aplication;
using HighRiskDiseaseSurvellance.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using OAuth.Adapter.WeChat.Options;
using OAuth.Adapter.WeChat.Requests;
using System;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Hosting;

var logger = LogManager.Setup().RegisterNLogWeb().GetCurrentClassLogger();
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<SurveillanceContext>();
    builder.Services.UserRedPacketService();
    builder.Services.AddWeChatAuth();
    builder.Services.Configure<WeChatOptions>(builder.Configuration.GetSection(nameof(WeChatOptions)));
    var mvcBuilder = builder.Services.AddControllersWithViews(options => options.Filters.Add(typeof(AppExceptionFilterAttribute)))
                            .AddJsonOptions(options =>
                                            {
                                                options.JsonSerializerOptions.NumberHandling =
                                                     JsonNumberHandling.AllowReadingFromString;
                                            });
    if (builder.Environment.IsDevelopment())
    {
        mvcBuilder.AddRazorRuntimeCompilation();
    }
    builder.Services.AddCors(options =>
                             {
                                 options.AddPolicy(name: MyAllowSpecificOrigins,
                                                   policyBuilder =>
                                                   {
                                                       policyBuilder.AllowAnyHeader();
                                                       policyBuilder.AllowAnyMethod();
                                                       policyBuilder.AllowAnyOrigin();
                                                   });
                             });
    string issuer;
    string audience;
    string signingKey;
    builder.Services.AddSingleton<ITokenProvider, JwtTokenProvider>();
    if (builder.Environment.IsDevelopment())
    {
        var jwtSection = builder.Configuration.GetSection("jwt");
        builder.Services.Configure<JwtOptions>(jwtSection);
        issuer     = jwtSection.GetValue<string>(nameof(JwtOptions.Issuer));
        audience   = jwtSection.GetValue<string>(nameof(JwtOptions.Audience));
        signingKey = jwtSection.GetValue<string>(nameof(JwtOptions.SigningKey));
    }
    else
    {
        builder.Services.Configure<JwtOptions>(option =>
                                               {
                                                   option.Issuer     = Environment.GetEnvironmentVariable("JWT_ISSUER");
                                                   option.Audience   = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
                                                   option.SigningKey = Environment.GetEnvironmentVariable("JWT_SIGNINGKEY");
                                                   int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRES"),
                                                                out var expires);
                                                   if (expires <= 0) expires = 120;
                                                   option.Expires = expires;
                                               });
        issuer     = Environment.GetEnvironmentVariable("JWT_ISSUER");
        audience   = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
        signingKey = Environment.GetEnvironmentVariable("JWT_SIGNINGKEY");
        logger.Info($"issuser:{issuer}, audience :{audience}, signingKey:{signingKey}");
    }
    
    builder.Services.AddAuthentication(options =>
                                       {
                                           options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                                           options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                                           options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                                       })
           .AddJwtBearer(options =>
                         {
                             options.TokenValidationParameters = new TokenValidationParameters
                             {
                                 ValidateIssuer = true,
                                 ValidateAudience = true,
                                 ValidateIssuerSigningKey = true,
                                 ValidIssuer = issuer,
                                 ValidAudience = audience,
                                 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey))
                             };

                         })
           .AddCookie(options =>
                      {
                          options.ExpireTimeSpan    = TimeSpan.FromMinutes(30);
                          options.SlidingExpiration = true;
                          options.LoginPath         = "/Account/Login";
                          options.AccessDeniedPath  = "/Home/Error";
                      });

    builder.Services.AddAuthorization();

    var app = builder.Build();
    app.UseCors(MyAllowSpecificOrigins);

    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllerRoute(
                           name: "default",
                           pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    //NLog: catch setup errors
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}