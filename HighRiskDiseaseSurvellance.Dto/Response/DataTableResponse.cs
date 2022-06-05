namespace HighRiskDiseaseSurvellance.Dto.Response
{
    public class DataTableResponse<T>
    {
        public int Draw { get; set; }

        public int RecordsTotal { get; set; }

        public int RecordsFiltered { get; set; }

        public T Data { get; set; }
    }
}
