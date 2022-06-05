using System.ComponentModel.DataAnnotations;

namespace HighRiskDiseaseSurvellance.Domain.Models.ValueObjects
{
    public class UserInfo : ValueObject
    {
        public UserInfo()
        {
            
        }

        [MaxLength(255)]
        public string WeChatOpenId { get; private set; }

        [MaxLength(255)]
        public string NickName { get; private set; }

        [MaxLength(255)]
        public string PhoneNumber { get; private set; }

        public UserInfo(string weChatOpenId, string nickName, string phoneNumber)
        {
            WeChatOpenId = weChatOpenId;
            NickName     = nickName;
            PhoneNumber  = phoneNumber;
        }
    }
}
