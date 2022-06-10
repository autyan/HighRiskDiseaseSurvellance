using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HighRiskDiseaseSurvellance.Domain.Models.ValueObjects;
using HighRiskDiseaseSurvellance.Dto;
using HighRiskDiseaseSurvellance.Dto.Models;
using HighRiskDiseaseSurvellance.Infrastructure;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    public class SurveillanceRecord : AppAggregateRoot
    {
        public SurveillanceRecord()
        {
            
        }

        [MaxLength(255)]
        public string Id { get; set; }

        public UserInfo UserInfo { get; set; }

        [Column(TypeName = "longtext")]
        public string SurveillanceContent { get; set; }

        [MaxLength(255)]
        public string SurveillanceTypeName { get; set; }

        [MaxLength(255)]
        public string OrderId { get; set; }

        [MaxLength(255)]
        public string UserId { get; set; }

        public SurveillanceRecordStatus Status { get; set; }

        public decimal Score { get; set; }

        public SurveillanceRecord(UserInfo userInfo, string surveillanceContent, string surveillanceTypeName, string userId)
        {
            Id                   = ObjectId.GenerateNewId().ToString();
            UserInfo             = userInfo;
            SurveillanceContent  = surveillanceContent;
            SurveillanceTypeName = surveillanceTypeName;
            UserId               = userId;
            Status               = SurveillanceRecordStatus.Unpaid;
        }

        public void ComputeScore(ISurveillance surveillance)
        {
            Score  = surveillance.Compute();
            Status = SurveillanceRecordStatus.Finished;
        }
    }
}
