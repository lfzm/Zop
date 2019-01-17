using System;

namespace Zop.Domain.Entities
{

    /// <summary>
    /// 聚合根的抽象实现类
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    [Serializable]
    public abstract class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>
    {
       
    }


}
