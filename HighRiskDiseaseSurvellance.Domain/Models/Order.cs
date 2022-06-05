using System.ComponentModel.DataAnnotations;
using HighRiskDiseaseSurvellance.Domain.Models.ValueObjects;
using HighRiskDiseaseSurvellance.Dto;
using HighRiskDiseaseSurvellance.Infrastructure;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    public class Order : AppAggregateRoot
    {
        public Order()
        {
            
        }

        [MaxLength(255)]
        public string Id { get; set; }

        [MaxLength(255)]
        public string OrderNumber { get; set; }

        public OrderPayStatus OrderPayStatus { get; set; }

        public UserInfo UserInfo { get; set; }

        [MaxLength(255)]
        public string UserId { get; set; }

        [Range(0, 10000)]
        public decimal Price { get; set; }

        public Order(UserInfo userInfo, string orderNumber, string userId, decimal price, OrderPayStatus orderPayStatus = OrderPayStatus.Unpaid)
        {
            Id             = ObjectId.GenerateNewId().ToString();
            OrderNumber    = orderNumber;
            OrderPayStatus = orderPayStatus;
            UserInfo       = userInfo;
            UserId         = userId;
            Price          = price;
        }
    }
}
