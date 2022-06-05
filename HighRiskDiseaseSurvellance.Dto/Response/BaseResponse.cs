namespace HighRiskDiseaseSurvellance.Dto.Response
{
    public class BaseResponse
    {
        public string Message { get; set; }

        public string ErrorCode { get; set; }

        public object Exception { get; set; }
    }

    public class BaseResponse<T> : BaseResponse
    {
        public BaseResponse(T data)
        {
            Data = data;
        }

        public T Data { get; set; }
    }
}
