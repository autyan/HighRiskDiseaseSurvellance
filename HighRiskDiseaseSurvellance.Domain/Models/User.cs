using System;
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

        public bool HasSyncWeChatUserProfile { get; set; }

        public User(string weChatOpenId,
                    bool   isDistributor     = false,
                    string distributorId     = null, 
                    string distributorQrCode = null)
        {
            Id                = ObjectId.GenerateNewId().ToString();
            NickName          = $"匿名用户：{Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0,8)}";
            WeChatOpenId      = weChatOpenId;
            DistributorId     = distributorId;
            IsDistributor     = isDistributor;
            DistributorQrCode = distributorQrCode;
            AvatarUrl =
                "https://7072-prod-0g7y3h923f261408-1311435230.tcb.qcloud.la/e12f955b3de3ae2c04121b6fcfc7f749.jpg?sign=b878bc165f8112c3783950d16aaf08e7&t=1656088018";
            HasSyncWeChatUserProfile = false;
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

        public void SetDistributor(string distributorId)
        {
            DistributorId = distributorId;
        }

        public void SyncWeChatUserProfile(string nickName, string avatarUrl)
        {
            NickName                 = nickName;
            AvatarUrl                = avatarUrl;
            HasSyncWeChatUserProfile = true;
        }
    }
}
