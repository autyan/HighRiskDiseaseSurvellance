using System;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    public interface IUpdated
    {
        /// <summary>
        /// 实体修改时间
        /// </summary>
        public DateTime? ModifiedTime { get; set; }
    }
}
