namespace HighRiskDiseaseSurvellance.Dto.Requests
{
    public class WeChatAuthRequest
    {
        public string Code { get; set; }

        public string DistributorId { get; set; }

        public string NickName { get; set; }

        public string AvatarUrl { get; set; }

        public string PhoneNumber { get; set; }
    }
}
