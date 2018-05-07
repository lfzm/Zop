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
        public IDictionary<string, ChangeEntry> EntryChangers = new Dictionary<string, ChangeEntry>();
        /// <summary>
        /// 实体添加信息
        /// </summary>
        public IList<ChangeEntry> AdditionEntryChangers = new List<ChangeEntry>();


        public void AddChanger(ChangeEntry change)
        {
            if (change.Type == ChangeEntryType.Addition)
                this.AdditionEntryChangers.Add(change);
            else
            {
                string key = change.EntryType.Name + change.Id;
                var c = this.GetChanger(change.EntryType, change.Id);
                if (c != null)
                    c = change;
                else
                    this.EntryChangers.Add(key, change);
            }
        }

        public void ClearChanger()
        {
            this.EntryChangers.Clear();
            this.AdditionEntryChangers.Clear();
        }

    
        public ChangeEntry GetChanger(Type type, object Id)
        {
            string key = type.Name + Id;
            if (!this.EntryChangers.ContainsKey(key))
                return null;
            return this.EntryChangers[key];
        }

        public IList<ChangeEntry> GetChangers(ChangeEntryType changeType)
        {
            if (changeType == ChangeEntryType.Addition)
                return AdditionEntryChangers;
            else
            {
                return this.EntryChangers.Values.ToList().Where(f => f.Type == changeType).ToList();
            }
        }
    }
}
