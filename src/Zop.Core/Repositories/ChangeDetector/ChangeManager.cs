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
        public IDictionary<object, ChangeEntry> ChangersEntry = new Dictionary<object, ChangeEntry>();
        /// <summary>
        /// 删除的实体
        /// </summary>
        public IList<ChangeEntry> DeleteEntry = new List<ChangeEntry>();


        public void AddChanger(ChangeEntry change)
        {
            if (change.Type == ChangeEntryType.Remove)
                this.DeleteEntry.Add(change);
            else
            {
                var c = this.GetChanger(change.NewestEntry);
                if (c != null)
                    c = change;
                else
                    this.ChangersEntry.Add(change.NewestEntry, change);
            }
        }

        public void ClearChanger()
        {
            this.ChangersEntry.Clear();
            this.DeleteEntry.Clear();
        }

    
        public ChangeEntry GetChanger(object obj)
        {
            if (!this.ChangersEntry.ContainsKey(obj))
                return null;
            return this.ChangersEntry[obj];
        }

        public IList<ChangeEntry> GetChangers(ChangeEntryType changeType)
        {
            if (changeType == ChangeEntryType.Remove)
                return DeleteEntry;
            else
            {
                return this.ChangersEntry.Values.ToList().Where(f => f.Type == changeType).ToList();
            }
        }
    }
}
