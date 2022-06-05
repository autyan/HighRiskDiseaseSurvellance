using HighRiskDiseaseSurvellance.Dto.Response;
using HighRiskDiseaseSurvellance.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace aspnetapp.Filters
{
    public class AppExceptionFilterAttribute: IExceptionFilter
    {
        private readonly ILogger<AppExceptionFilterAttribute> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public AppExceptionFilterAttribute(ILogger<AppExceptionFilterAttribute> logger, 
                                           IWebHostEnvironment                  hostingEnvironment)
        {
            _logger             = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                             context.Exception,
                             context.Exception.Message);
            if (context.Exception is DomainException domainException)
            {
                var errorResponse = new BaseResponse
                {
                    Message = domainException.Message,
                    ErrorCode = domainException.ErrorCode.ToString()
                };

                if (_hostingEnvironment.IsDevelopment())
                {
                    errorResponse.Exception = new
                    {
                        domainException.Message,
                        domainException.StackTrace,
                        domainException.ErrorCode,
                        Class = domainException.GetType().Name
                    };
                }
                // 400 错误
                context.Result = new BadRequestObjectResult(errorResponse);
            }
            else
            {
                
                var errorResponse = new BaseResponse
                {
                    Message = "系统错误， 请再尝试。"
                };

                if (_hostingEnvironment.IsDevelopment() || _hostingEnvironment.IsStaging())
                {
                    errorResponse.Exception = new
                    {
                        context.Exception.GetBaseException().Message,
                        context.Exception.StackTrace,
                        Class = context.Exception.GetType().Name
                    };
                }
                // 500 错误
                context.Result = new ObjectResult(errorResponse) { StatusCode = StatusCodes.Status500InternalServerError };
            }
            context.ExceptionHandled = true;
        }
    }
}
