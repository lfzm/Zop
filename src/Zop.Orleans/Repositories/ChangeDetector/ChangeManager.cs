using KellermanSoftware.CompareNetObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    public class ChangeManager : IChangeManager
    {
        /// <summary>
        /// 实体变动信息
        /// </summary>
        public readonly EntityChange _entityChange;
        public ChangeManager(EntityChange entityChange)
        {
            this._entityChange = entityChange;
        }

        public EntityChange GetChange()
        {
            return _entityChange;
        }

        public IList<EntityDifference> GetDifferences(EntityChangeType changeType)
        {
            if (changeType == EntityChangeType.Remove)
                return _entityChange?.DeleteEntry;
            else
            {
                return _entityChange?.ChangeDifference.Values.ToList().Where(f => f.Type == changeType).ToList();
            }
        }
        public EntityDifference GetDifference(int entityHashCode)
        {
            if (_entityChange.ChangeDifference.ContainsKey(entityHashCode))
                return _entityChange.ChangeDifference[entityHashCode];
            else
                return null;
        }
    }
}
