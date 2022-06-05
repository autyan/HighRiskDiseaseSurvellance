using System;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    public interface ICreated
    {
        /// <summary>
        /// 实体创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
