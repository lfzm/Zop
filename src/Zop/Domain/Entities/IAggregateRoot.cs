namespace Zop.Domain.Entities
{

    /// <summary>
    /// 聚合根接口
    /// </summary>
    /// <typeparam name="TPrimaryKey">聚合根的唯一标示类型</typeparam>
    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>         
    {

    }

}
