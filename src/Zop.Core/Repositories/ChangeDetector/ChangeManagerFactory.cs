
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using Zop.Domain.Values;

namespace Zop.Repositories.ChangeDetector
{
    public class ChangeManagerFactory : IChangeManagerFactory
    {
        private readonly IChangeManager changeManager;
        public ChangeManagerFactory(IChangeManager _changeManager)
        {
            this.changeManager = _changeManager;
        }
        public IChangeManager Create(ComparisonResult comparisonResult)
        {
            changeManager.ClearChanger();
            if (comparisonResult.AreEqual)
                return changeManager;

            //根据实体与快照的差异进行分析实体的变动
            foreach (var difference in comparisonResult.Differences)
            {
                ChangeEntry change = this.ResolveChangeEntry(difference);
                if (change.Type == ChangeEntryType.Addition || change.Type == ChangeEntryType.Remove)
                {
                    this.changeManager.AddChanger(change);
                    if (change.Type == ChangeEntryType.Addition)
                    {
                        //如果为新增类型，需要进一步分析实体中的值对象
                        var values = this.ResolveChangeObjectValue(change);
                        foreach (var item in values)
                        {
                            this.changeManager.AddChanger(item);
                        }
                    }
                }
                else
                {
                    var change2 = this.changeManager.GetChanger(change.NewestEntry);
                    if (change2 == null)
                    {
                        this.changeManager.AddChanger(change);
                        change2 = change;
                    }


                    //解析修改的字段
                    ChangeEntryPropertys changeProperty = this.ResolveChangeEntryPropertys(difference);
                    change2.ChangePropertys.Add(changeProperty);
                }
            }
            return changeManager;
        }

        /// <summary>
        /// 根据对比差异分析变动实体的变动类型
        /// </summary>
        /// <param name="difference"></param>
        /// <returns></returns>
        private ChangeEntryType ResolveChangeType(Difference difference)
        {
            if (difference.Object2Value == "_ADD_")
                return ChangeEntryType.Addition;
            else if (difference.Object1Value == "_DEL_")
                return ChangeEntryType.Remove;
            else
                return ChangeEntryType.Modify;
        }
        /// <summary>
        /// 根据对比差异分析变动实体
        /// </summary>
        /// <param name="difference"></param>
        /// <returns></returns>
        private ChangeEntry ResolveChangeEntry(Difference difference)
        {
            ChangeEntry change = new ChangeEntry();
            change.Type = this.ResolveChangeType(difference);
            if (change.Type == ChangeEntryType.Addition || change.Type == ChangeEntryType.Remove)
            {
                change.OriginalEntry = difference.Object1;
                change.NewestEntry = difference.Object2;
            }
            else
            {
                change.OriginalEntry = difference.ParentObject1;
                change.NewestEntry = difference.ParentObject2;
            }
            change.Navigation = difference.ParentPropertyName;
            return change;
        }

        /// <summary>
        /// 分析变动实体的变动属性
        /// </summary>
        /// <param name="difference"></param>
        /// <returns></returns>
        private ChangeEntryPropertys ResolveChangeEntryPropertys(Difference difference)
        {
            ChangeEntryPropertys change = new ChangeEntryPropertys();
            change.NewestValue = difference.Object2Value;
            change.OriginalValue = difference.Object1Value;
            change.Name = difference.PropertyName.Substring(difference.PropertyName.LastIndexOf(".") + 1);
            return change;
        }
        /// <summary>
        /// 分析变动实体中值对象
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        private List<ChangeEntry> ResolveChangeObjectValue(ChangeEntry entry)
        {
            var objectValues = entry.EntryType.GetProperties()
                  .Where(f => typeof(IValueObject).IsAssignableFrom(f.PropertyType))
                  .ToList();

            List<ChangeEntry> entries = new List<ChangeEntry>();
            foreach (var ov in objectValues)
            {
                var value = ov.GetValue(entry.NewestEntry);
                if (value == null)
                    continue;

                ChangeEntry change = new ChangeEntry();
                change.Type = ChangeEntryType.Addition;
                change.NewestEntry = value;
                entries.Add(change);
            }
            return entries;
        }
    }
}
