namespace HighRiskDiseaseSurvellance.Dto.Requests
{
    public class UserLoginRequest
    {
        public string WeChatOpenId { get; set; }

        public string DistributorId { get; set; }

        public string NickName { get; set; }

        public string AvatarUrl { get; set; }

        public string PhoneNumber { get; set; }
    }
}
