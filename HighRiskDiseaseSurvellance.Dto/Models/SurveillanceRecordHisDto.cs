using System;
using System.Text.Json.Serialization;
using aspnetapp.JsonConverters;

namespace HighRiskDiseaseSurvellance.Dto.Models
{
    public class SurveillanceRecordHisDto
    {
        public string Id { get; set; }

        public string SurveillanceTypeName { get; set; }

        public string SurveillanceTypeDisplayName { get; set; }

        public string OrderId { get; set; }

        [JsonConverter(typeof(DisplayDateTimeConverter))]
        public DateTime CreateTime { get; set; }

        public SurveillanceRecordStatus Status { get; set; }
    }
}
