using System;

namespace HighRiskDiseaseSurvellance.Domain.Models
{
    /// <summary>
    /// 实体类
    /// </summary>
    public class Entity : ICreated, IUpdated
    {
        public DateTime  CreateTime   { get; set; }
        public DateTime? ModifiedTime { get; set; }

        public void UpdateCreation()
        {
            ModifiedTime = CreateTime;
        }

        public void UpdateModification()
        {
            ModifiedTime = DateTime.Now;
        }
    }
}
