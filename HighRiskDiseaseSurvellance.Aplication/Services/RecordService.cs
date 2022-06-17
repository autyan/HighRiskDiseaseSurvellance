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
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HighRiskDiseaseSurvellance.Aplication.Services
{
    public class RecordService : BaseService
    {
        public RecordService(SurveillanceContext dbContext, 
                             ILogger<RecordService> logger) : base(dbContext, logger)
        {
            
        }

        public async Task<string> SubmitSurveillanceRecordAsync(SubmitRecordRequest request)
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

            var surveillance = ConvertRecordContentToSurveillance(request.RecordTypeName, request.RecordContent);
            if (surveillance != null)
            {
                record.ComputeScore(surveillance);
            }
            DbContext.Records.Add(record);
            await DbContext.SaveChangesAsync();
            return record.Id;
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

        public Task<SurveillanceRecordDto> GetRecordAsync(string id)
        {
            return DbContext.Records.Select(r => new SurveillanceRecordDto
                                                 {
                                                     Id=r.Id,
                                                     CreateTime = r.CreateTime,
                                                     OrderId = r.OrderId,
                                                     SurveillanceContent = r.SurveillanceContent,
                                                     SurveillanceTypeName = r.SurveillanceTypeName,
                                                     Score = r.Score
                                                 }).FirstOrDefaultAsync(r => r.Id == id);
        }

        private ISurveillance ConvertRecordContentToSurveillance(string name, string content)
        {
            switch (name)
            {
                case SurveillanceNames.Hyperlipidemia:
                    return JsonSerializer.Deserialize<Hyperlipidemia>(content, new JsonSerializerOptions
                                                                               {
                                                                                   NumberHandling = JsonNumberHandling.AllowReadingFromString
                                                                               });
                case SurveillanceNames.Hypertension:
                    return JsonSerializer.Deserialize<Hypertension>(content, new JsonSerializerOptions
                                                                             {
                                                                                 NumberHandling = JsonNumberHandling.AllowReadingFromString
                                                                             } );
                case SurveillanceNames.AnginaPectoris:
                    return JsonSerializer.Deserialize<AnginaPectoris>(content, new JsonSerializerOptions
                                                                               {
                                                                                   NumberHandling = JsonNumberHandling.AllowReadingFromString
                                                                               } );
                case SurveillanceNames.CardiacInsufficiency:
                    return JsonSerializer.Deserialize<CardiacInsufficiency>(content, new JsonSerializerOptions
                                                                                     {
                                                                                         NumberHandling = JsonNumberHandling.AllowReadingFromString
                                                                                     } );
                default : return null;
            }
        }
    }
}
