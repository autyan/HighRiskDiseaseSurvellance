namespace HighRiskDiseaseSurvellance.Dto.Requests
{
    public class UserQuery : DataTableQuery
    {
        public string NickName { get; set; }

        public bool? IsDistributor { get; set; }
    }
}
