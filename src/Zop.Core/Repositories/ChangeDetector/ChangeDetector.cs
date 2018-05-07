
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
        public IChangeManager DetectChanges(object newEntry, object oldEntry)
        {
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.MaxDifferences = int.MaxValue;
            compareLogic.Config.CustomComparers.Add(new ChangeCollectionComparer(RootComparerFactory.GetRootComparer()));
            var result = compareLogic.Compare(oldEntry, newEntry);
            
            return this.changeManagerFactory.Create(result);
        }
    }
}
