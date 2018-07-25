using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 实体差异
    /// </summary>
    public class EntityDifference
    {
        public EntityDifference(string entityType, EntityChangeType type, string navigation)
        {
            this.EntityType = entityType;
            this.Type = type;
            this.Navigation = navigation;
        }
        /// <summary>
        /// 实体类型
        /// </summary>
        public string EntityType { get; private set; }
        /// <summary>
        /// 实体变动类型
        /// </summary>
        public EntityChangeType Type { get; private set; }
        /// <summary>
        /// 导航属性
        /// </summary>
        public string Navigation { get; private set; }
        /// <summary>
        /// 变动属性集合
        /// </summary>
        public IList<EntityChangePropertys> ChangePropertys { get; } = new List<EntityChangePropertys>();
    }
}
