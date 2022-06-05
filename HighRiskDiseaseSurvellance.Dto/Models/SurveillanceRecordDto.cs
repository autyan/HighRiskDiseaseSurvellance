using System;

namespace HighRiskDiseaseSurvellance.Dto.Models
{
    public class SurveillanceRecordDto
    {
        public string Id { get; set; }

        public string SurveillanceContent { get; set; }

        public string SurveillanceTypeName { get; set; }

        public string OrderId { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
