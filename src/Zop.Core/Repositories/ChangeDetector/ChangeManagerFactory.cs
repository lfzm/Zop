
using System;
using System.Collections.Generic;
using System.Text;
using KellermanSoftware.CompareNetObjects;

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
                    this.changeManager.AddChanger(change);
                else
                {
                    var change2 = this.changeManager.GetChanger(change.EntryType, change.Id);
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
        /// 解析获取变动类型
        /// </summary>
        /// <param name="difference"></param>
        /// <returns></returns>
        private ChangeEntryType ResolveChangeType(Difference difference)
        {
            if (difference.MessagePrefix == "_Add_")
                return ChangeEntryType.Addition;
            else if (difference.MessagePrefix == "_DEL_")
                return ChangeEntryType.Remove;
            else
                return ChangeEntryType.Modify;
        }
        /// <summary>
        /// 解析获取实体变动信息
        /// </summary>
        /// <param name="difference"></param>
        /// <returns></returns>
        private ChangeEntry ResolveChangeEntry(Difference difference)
        {
            ChangeEntry change = new ChangeEntry();
            change.Type = this.ResolveChangeType(difference);
            if(change.Type == ChangeEntryType.Addition || change.Type == ChangeEntryType.Remove)
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

        private ChangeEntryPropertys ResolveChangeEntryPropertys(Difference difference)
        {
            ChangeEntryPropertys change = new ChangeEntryPropertys();
            change.NewestValue = difference.Object2Value;
            change.OriginalValue = difference.Object1Value;
            change.Name = difference.PropertyName.Substring(difference.PropertyName.LastIndexOf(".") + 1);
            return change;
        }
    }
}
