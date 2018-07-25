using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 实体属性变动对象
    /// </summary>
    public class EntityChangePropertys
    {
        public EntityChangePropertys(string name, object oldValue, object newValue)
        {
            this.Name = name;
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }
        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 原来的值
        /// </summary>
        public object OldValue { get; private set; }
        /// <summary>
        /// 新的值
        /// </summary>
        public object NewValue { get; private set; }
    }
}
