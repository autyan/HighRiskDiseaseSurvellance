namespace HighRiskDiseaseSurvellance.Dto.Response
{
    public class RegisterUser
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public string AvatarUrl { get; set; }

        public bool IsDistributor { get; set; }
    }
}
