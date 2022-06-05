using HighRiskDiseaseSurvellance.Aplication.Services;
using HighRiskDiseaseSurvellance.Dto.Models;
using HighRiskDiseaseSurvellance.Dto.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Dto.Response;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace aspnetapp.Controllers.Api
{
    public class RecordController : ApiController
    {
        private readonly RecordService _recordService;

        public RecordController(RecordService recordService)
        {
            _recordService = recordService;
        }

        [HttpPost]
        public Task SubmitSurveillanceRecord([FromBody]SubmitRecordRequest request)
        {
            request.UserId = UserId;
            return _recordService.SubmitSurveillanceRecordAsync(request);
        }

        [HttpGet]
        public Task<SurveillanceRecordHisDto[]> QueryUserSurveillanceRecord([FromQuery]UserRecordQuery query)
        {
            if (UserId == null) return Task.FromResult<SurveillanceRecordHisDto[]>(null);
            query.UserId = UserId;
            return _recordService.QueryUserRecordsAsync(query);
        }

        [HttpGet("query")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public Task<DataTableResponse<SurveillanceRecordQueryDto[]>> QuerySurveillanceRecord([FromQuery] UserRecordQuery query)
        {
            return _recordService.QueryRecordsAsync(query);
        }
    }
}
