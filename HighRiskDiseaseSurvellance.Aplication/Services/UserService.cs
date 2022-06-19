using System;
using System.Linq;
using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Domain.Models;
using HighRiskDiseaseSurvellance.Dto;
using HighRiskDiseaseSurvellance.Dto.Models;
using HighRiskDiseaseSurvellance.Dto.Requests;
using HighRiskDiseaseSurvellance.Dto.Response;
using HighRiskDiseaseSurvellance.Infrastructure;
using HighRiskDiseaseSurvellance.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OAuth.Adapter.WeChat;

namespace HighRiskDiseaseSurvellance.Aplication.Services
{
    public class UserService : BaseService
    {
        private readonly IWeChatOAuthService _weChatOAuthService;
        
        public UserService(SurveillanceContext dbContext, ILogger<UserService> logger, IWeChatOAuthService weChatOAuthService) : base(dbContext, logger)
        {
            _weChatOAuthService = weChatOAuthService;
        }

        public async Task<AppUserBaseInfo> UserLoginAsync(UserLoginRequest request)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.WeChatOpenId == request.WeChatOpenId);
            if (user == null)
            {
                user = new User(request.NickName, request.PhoneNumber, request.WeChatOpenId, request.AvatarUrl, distributorId: request.DistributorId);

                await DbContext.AddAsync(user);
                await DbContext.SaveChangesAsync();
            }

            return new AppUserBaseInfo
                   {
                       Id = user.Id,
                       NickName = user.NickName,
                       AvatarUrl = user.AvatarUrl,
                       PhoneNumber = user.PhoneNumber,
                       IsDistributor = user.IsDistributor,
                       DistributorQrCode = user.DistributorQrCode
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

        public async Task<bool> MakeUserDistributorAsync(string id)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new DomainException(ErrorCode.UserNotFound);
            }

            var scene        = user.Id;
            var qrCode       = await _weChatOAuthService.GetUnlimitedCodeAsync(scene);
            var qrCodeBase64 = Convert.ToBase64String(qrCode);

            user.MakeDistributor(qrCodeBase64);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CancelUserDistributorAsync(string id)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new DomainException(ErrorCode.UserNotFound);
            }
            
            user.CancelDistributor();
            await DbContext.SaveChangesAsync();
            return true;
        }
    }
}
