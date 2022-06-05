using System;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    public class AppAggregateRoot : Entity
    {
        protected AppAggregateRoot()
        {
            CreateTime = DateTime.Now;
        }
    }
}
