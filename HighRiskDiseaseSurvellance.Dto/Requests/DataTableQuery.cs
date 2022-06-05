namespace HighRiskDiseaseSurvellance.Dto.Requests
{
    public class DataTableQuery
    {
        public int Draw { get; set; }

        public int Start { get; set; } = 0;

        public int Length { get; set; } = 20;

        public int Take => Length;

        public int Skip => Start;
    }
}
