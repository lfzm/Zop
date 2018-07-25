using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Zop.Repositories.ChangeDetector
{
    /// <summary>
    /// 实体变动
    /// </summary>
    public class EntityChange
    {
        public EntityChange(object oldEntity, object newEntity, int versionNo)
        {
            this.OldEntity = oldEntity;
            this.NewEntity = newEntity;
            this.VersionNo = versionNo;
        }
        /// <summary>
        /// 旧对象
        /// </summary>
        [JsonProperty]
        public object OldEntity { get; private set; }
        /// <summary>
        /// 新对象
        /// </summary>
        [JsonProperty]
        public object NewEntity { get; private set; }
        /// <summary>
        /// 版本号
        /// </summary>
        [JsonProperty]
        public int VersionNo { get; private set; }
        /// <summary>
        /// 变动时间
        /// </summary>
        [JsonProperty]
        public DateTime ChangeTime { get; private set; } = DateTime.Now;

        /// <summary>
        /// 实体变动信息
        /// </summary>
        [JsonProperty]
        public IDictionary<string, EntityDifference> ChangeDifference { get; private set; } = new Dictionary<string, EntityDifference>();
        /// <summary>
        /// 删除的实体
        /// </summary>
        public IList<EntityDifference> DeleteEntry { get; private set; } = new List<EntityDifference>();




    }
}
