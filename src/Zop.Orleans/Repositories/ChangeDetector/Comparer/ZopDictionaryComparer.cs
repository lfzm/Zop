using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System;
using System.Collections.Generic;
using System.Text;
using Zop.Domain.Values;
using System.Linq;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    public class ZopDictionaryComparer : DictionaryComparer
    {
        public ZopDictionaryComparer(RootComparer rootComparer) : base(rootComparer)
        {

        }

        public override bool IsTypeMatch(Type type1, Type type2)
        {
            if (!base.IsTypeMatch(type1, type2))
                return false;

            if (type1.GetProperties().ToList().Exists(f => f.Name == "Json" && f.PropertyType == typeof(string)) &&
                  type2.GetProperties().ToList().Exists(f => f.Name == "Json" && f.PropertyType == typeof(string)))
                return true;

            else
                return false;

        }
        public override void CompareType(CompareParms parms)
        {

            var value1 = parms.Object1.GetType().GetProperties().FirstOrDefault(f => f.Name == "Json").GetValue(parms.Object1)?.ToString();
            var value2 = parms.Object2.GetType().GetProperties().FirstOrDefault(f => f.Name == "Json").GetValue(parms.Object2)?.ToString();
            //判断Json值是否一样
            if (value1 == value2)
                return;

            Difference difference = new Difference
            {
                Object1 = value1,
                Object2 = value2,
                Object1Value = value1,
                Object2Value = value2,
                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                PropertyName = "Json"
            };
            AddDifference(parms.Result, difference);

        }
    }
}
