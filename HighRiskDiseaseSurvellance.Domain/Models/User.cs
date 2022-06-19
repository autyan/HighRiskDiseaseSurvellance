using HighRiskDiseaseSurvellance.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    public class User : AppAggregateRoot
    {
        public User()
        {
            
        }

        [MaxLength(255)]
        public string Id { get; set; }

        [MaxLength(255)]
        public string NickName { get; set; }

        [MaxLength(2048)]
        public string AvatarUrl { get; set; }

        [MaxLength(255)]
        public string PhoneNumber { get; set; }

        [MaxLength(255)]
        public string WeChatOpenId { get; set; }

        [MaxLength(255)]
        public string DistributorId { get; set; }

        public bool IsDistributor { get; set; }

        [MaxLength(20000)]
        public string DistributorQrCode { get; set; }

        public User(string nickName,     string phoneNumber,
                    string weChatOpenId, string avatarUrl,
                    bool   isDistributor = false,
                    string distributorId = null, string distributorQrCode = null)
        {
            Id                = ObjectId.GenerateNewId().ToString();
            NickName          = nickName;
            PhoneNumber       = phoneNumber;
            WeChatOpenId      = weChatOpenId;
            AvatarUrl         = avatarUrl;
            DistributorId     = distributorId;
            IsDistributor     = isDistributor;
            DistributorQrCode = distributorQrCode;
        }

        public void MakeDistributor(string qrCode)
        {
            IsDistributor     = true;
            DistributorQrCode = qrCode;
        }

        public void CancelDistributor()
        {
            IsDistributor     = false;
            DistributorQrCode = null;
        }
    }
}
