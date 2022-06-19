using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Aplication.Services;
using HighRiskDiseaseSurvellance.Dto.Requests;
using HighRiskDiseaseSurvellance.Dto.Response;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetapp.Controllers.Api
{
    public class UserController : ApiController
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public Task<DataTableResponse<RegisterUser[]>> GetUsers([FromQuery] UserQuery query)
        {
            return _userService.QueryUsers(query);
        }

        [HttpPost("Distributor/{id}")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public Task<bool> MakeDistributor(string id)
        {
            return _userService.MakeUserDistributorAsync(id);
        }
        
        [HttpDelete("Distributor/{id}")]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public Task<bool> CancelDistributor(string id)
        {
            return _userService.CancelUserDistributorAsync(id);
        }
    }
}
