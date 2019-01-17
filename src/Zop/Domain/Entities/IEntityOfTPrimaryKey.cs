using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Domain.Entities
{
    /// <summary>
    ///定义基本实体类型的接口。 系统中的所有实体都必须实现这个接口。
    /// </summary>
    /// <typeparam name="TPrimaryKey">实体主键的类型</typeparam>
    public interface IEntity<TPrimaryKey>: IEntity
    {
        /// <summary>
        /// 此实体的唯一标识符。
        /// </summary>
        TPrimaryKey Id { get; }

     
    }
}
