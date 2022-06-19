namespace HighRiskDiseaseSurvellance.Dto.Models
{
    public class AppUserBaseInfo
    {
        public string Id { get; set; }

        public string NickName { get; set; }

        public string AvatarUrl { get; set; }

        public string PhoneNumber { get; set; }
        
        public bool IsDistributor { get; set; }

        public string DistributorQrCode { get; set; }
    }
}
