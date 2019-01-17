using System;
using System.Collections.Generic;
using System.Text;

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
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        TEntity Clone<TEntity>();

        /// <summary>
        /// 获取唯一标示
        /// </summary>
        /// <returns></returns>
        object GetPrimaryKey();

    }
}
