using System.Linq;
using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Domain.Models;
using HighRiskDiseaseSurvellance.Dto.Models;
using HighRiskDiseaseSurvellance.Dto.Requests;
using HighRiskDiseaseSurvellance.Dto.Response;
using HighRiskDiseaseSurvellance.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HighRiskDiseaseSurvellance.Aplication.Services
{
    public class UserService : BaseService
    {
        public UserService(SurveillanceContext dbContext, ILogger<UserService> logger) : base(dbContext, logger)
        {
        }

        public async Task<AppUserBaseInfo> UserLoginAsync(UserLoginRequest request)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.WeChatOpenId == request.WeChatOpenId);
            if (user == null)
            {
                user = new User(request.NickName, request.PhoneNumber, request.WeChatOpenId, request.AvatarUrl);

                await DbContext.AddAsync(user);
                await DbContext.SaveChangesAsync();
            }

            return new AppUserBaseInfo
                   {
                       Id = user.Id,
                       NickName = user.NickName,
                       AvatarUrl = user.AvatarUrl,
                       PhoneNumber = user.PhoneNumber,
                   };
        }

        public async Task<DataTableResponse<RegisterUser[]>> QueryUsers(UserQuery query)
        {
            var total      = await DbContext.Users.CountAsync();
            var userQuery  = DbContext.Users.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.NickName))
            {
                userQuery = userQuery.Where(u => u.NickName == query.NickName);
            }

            if (query.IsDistributor != null)
            {
                userQuery = userQuery.Where(u => u.IsDistributor == query.IsDistributor.Value);
            }
            var queryTotal = await userQuery.CountAsync();
            var users      = userQuery.Skip(query.Skip).Take(query.Take);
            var queriedUsers = await users.Select(u => new RegisterUser
                                                       {
                                                           Id            = u.Id,
                                                           AvatarUrl     = u.AvatarUrl,
                                                           NickName      = u.NickName,
                                                           IsDistributor = u.IsDistributor
                                                       }).ToArrayAsync();

            return new DataTableResponse<RegisterUser[]>
                   {
                       Draw            = query.Draw,
                       RecordsTotal    = total,
                       RecordsFiltered = queryTotal,
                       Data            = queriedUsers
                   };

        }
    }
}
