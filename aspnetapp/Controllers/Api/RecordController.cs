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
        public async Task<BaseResponse<string>> SubmitSurveillanceRecord([FromBody]SubmitRecordRequest request)
        {
            request.UserId = UserId;
            var recordId = await  _recordService.SubmitSurveillanceRecordAsync(request);
            return new BaseResponse<string>(recordId);
        }

        [HttpGet("history")]
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

        [HttpGet("{id}")]
        public Task<SurveillanceRecordDto> GetUserRecord(string id)
        {
            return _recordService.GetRecordAsync(id);
        }
    }
}
