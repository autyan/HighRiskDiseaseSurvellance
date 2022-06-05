namespace HighRiskDiseaseSurvellance.Dto.Requests
{
    public class UserRecordQuery : DataTableQuery
    {
        public string UserId { get; set; }

        public SurveillanceRecordStatus? Status { get; set; }

        public string SurveillanceTypeName { get; set; }
    }
}
