using System.Linq;
using System.Threading.Tasks;
using HighRiskDiseaseSurvellance.Domain.Models;
using HighRiskDiseaseSurvellance.Domain.Models.ValueObjects;
using HighRiskDiseaseSurvellance.Dto;
using HighRiskDiseaseSurvellance.Dto.Models;
using HighRiskDiseaseSurvellance.Dto.Requests;
using HighRiskDiseaseSurvellance.Dto.Response;
using HighRiskDiseaseSurvellance.Infrastructure;
using HighRiskDiseaseSurvellance.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HighRiskDiseaseSurvellance.Aplication.Services
{
    public class RecordService : BaseService
    {
        public RecordService(SurveillanceContext dbContext, 
                             ILogger<RecordService> logger) : base(dbContext, logger)
        {
            
        }

        public async Task SubmitSurveillanceRecordAsync(SubmitRecordRequest request)
        {
            var user = await DbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId);
            if (user == null)
            {
                throw new DomainException(ErrorCode.UserNotFound);
            }

            var userInfo = new UserInfo(user.WeChatOpenId, user.NickName, user.PhoneNumber);

            var record = new SurveillanceRecord(userInfo,
                                                request.RecordContent,
                                                request.RecordTypeName,
                                                user.Id);
            DbContext.Records.Add(record);
            await DbContext.SaveChangesAsync();
        }

        public async Task<SurveillanceRecordHisDto[]> QueryUserRecordsAsync(UserRecordQuery query)
        {
            var queryBuilder = DbContext.Records.Where(r => r.UserId == query.UserId);
            if (query.Status != null)
            {
                queryBuilder = queryBuilder.Where(r => r.Status == query.Status);
            }

            return await queryBuilder.Select(r => new SurveillanceRecordHisDto
                                                  {
                                                      Id                   = r.Id,
                                                      SurveillanceTypeName = r.SurveillanceTypeName,
                                                      SurveillanceTypeDisplayName = SurveillanceNames.GetSurveillanceTypeName(r.SurveillanceTypeName),
                                                      CreateTime           = r.CreateTime,
                                                      OrderId              = r.OrderId,
                                                      Status               = r.Status
                                                  })
                                     .ToArrayAsync();
        }

        public async Task<DataTableResponse<SurveillanceRecordQueryDto[]>> QueryRecordsAsync(UserRecordQuery query)
        {
            var total      = await DbContext.Records.CountAsync();
            var recordsQuery  = DbContext.Records.AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.UserId))
            {
                recordsQuery = recordsQuery.Where(r => r.UserId == query.UserId);
            }

            if (query.Status != null)
            {
                recordsQuery = recordsQuery.Where(r => r.Status == query.Status);
            }

            if (!string.IsNullOrWhiteSpace(query.SurveillanceTypeName))
            {
                recordsQuery = recordsQuery.Where(r => r.SurveillanceTypeName == query.SurveillanceTypeName);
            }

            var queryTotal = await recordsQuery.CountAsync();
            var records      = recordsQuery.Skip(query.Skip).Take(query.Take);
            var queriedUsers = await records.Select(r => new SurveillanceRecordQueryDto
                                                       {
                                                           Id            = r.Id,
                                                           UserNickName = r.UserInfo.NickName,
                                                           SurveillanceTypeName = r.SurveillanceTypeName,
                                                           SurveillanceTypeDisplayName = SurveillanceNames.GetSurveillanceTypeName(r.SurveillanceTypeName),
                                                           CreateTime           = r.CreateTime,
                                                           OrderId              = r.OrderId,
                                                           Status               = r.Status
                                                       }).ToArrayAsync();

            return new DataTableResponse<SurveillanceRecordQueryDto[]>
                   {
                       Draw            = query.Draw,
                       RecordsTotal    = total,
                       RecordsFiltered = queryTotal,
                       Data            = queriedUsers
                   };
        }
    }
}
