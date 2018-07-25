
using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Repositories.ChangeDetector
{
    public class ChangeDetector : IChangeDetector
    {
        private readonly IChangeManagerFactory changeManagerFactory;

        public ChangeDetector(IChangeManagerFactory _changeManagerFactory)
        {
            this.changeManagerFactory = _changeManagerFactory;
        }
        public IChangeManager DetectChanges(object newEntry, object oldEntry, int versionNo)
        {
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = int.MaxValue;
            compareLogic.Config.CompareStaticFields = false;//静态字段不比较
            compareLogic.Config.CompareStaticProperties = false;//静态属性不比较
            compareLogic.Config.Caching = true;
            compareLogic.Config.CustomComparers.Add(new ZopDictionaryComparer(RootComparerFactory.GetRootComparer()));
            compareLogic.Config.CustomComparers.Add(new ZopListComparer(RootComparerFactory.GetRootComparer()));
            compareLogic.Config.CustomComparers.Add(new EntityCollectionComparer(RootComparerFactory.GetRootComparer()));

            var result = compareLogic.Compare(oldEntry, newEntry);
            var entityChange = new EntityChange(oldEntry, newEntry, versionNo);
            return this.changeManagerFactory.Create(entityChange, result);
        }
    }
}
