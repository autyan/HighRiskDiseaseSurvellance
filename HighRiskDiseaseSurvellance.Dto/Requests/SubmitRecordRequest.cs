namespace HighRiskDiseaseSurvellance.Dto.Requests
{
    public class SubmitRecordRequest
    {
        public string UserId { get; set; }

        public string RecordContent { get; set; }

        public string RecordTypeName { get; set; }
    }
}
