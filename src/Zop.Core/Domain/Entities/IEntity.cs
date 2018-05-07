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
        /// 检查这个实体是否是临时的。
        /// </summary>
        /// <returns>True: 这个实体是临时的</returns>
        bool IsTransient { get;  }

        /// <summary>
        /// 深度复制
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        TEntity Clone<TEntity>();
    }
}
