using System;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 线程安全实体基类
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    [Serializable]                                                                                                                            
    public abstract class EntityConcurrencySafe<TPrimaryKey> : Entity<TPrimaryKey>, IEntity<TPrimaryKey>, IConcurrencySafe
    {
        /// <summary>
        /// 乐观锁 版本号
        /// </summary>
        public int VersionNo { get; set; }
    }
 
}
