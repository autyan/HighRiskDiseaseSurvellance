namespace HighRiskDiseaseSurvellance.Dto.Requests
{
    public class PagedQuery
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int Take => PageSize;

        public int Skip => PageSize * PageNumber;
    }
}
