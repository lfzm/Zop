using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Zop.Domain.Entities
{
    /// <summary>
    /// 标示领域实体基类
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 深度复制
        /// </summary>
        /// <typeparam name="TEntity">赋值类型</typeparam>
        /// <returns></returns>
        TEntity Clone<TEntity>();
     
     
    }
}
