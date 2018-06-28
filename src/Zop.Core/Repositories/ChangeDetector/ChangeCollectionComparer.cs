using KellermanSoftware.CompareNetObjects;
using KellermanSoftware.CompareNetObjects.IgnoreOrderTypes;
using KellermanSoftware.CompareNetObjects.TypeComparers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Zop.Domain.Entities;

namespace KellermanSoftware.CompareNetObjects.TypeComparers
{
    public class ChangeCollectionComparer : CollectionComparer
    {
        public ChangeCollectionComparer(RootComparer rootComparer) : base(rootComparer)
        {
        }
        public override bool IsTypeMatch(Type type1, Type type2)
        {
            bool isTypeMatch = base.IsTypeMatch(type1, type2);
            if (isTypeMatch)
            {
                if (type1.GenericTypeArguments.Count() == 0)
                    return false;
                if (!typeof(IEntity).IsAssignableFrom(type1.GenericTypeArguments[0]))
                    return false;
            }
            return isTypeMatch;
        }
        public override void CompareType(CompareParms parms)
        {
            Type t1 = parms.Object1.GetType();
            Type t2 = parms.Object2.GetType();

            //Check if the class type should be excluded based on the configuration
            if (ExcludeLogic.ShouldExcludeClass(parms.Config, t1, t2))
                return;

            parms.Object1Type = t1;
            parms.Object2Type = t2;

            if (!parms.Config.IgnoreCollectionOrder)
            {
                CompareItems(parms);
            }

        }
        /// <summary>
        /// 分析List中的对象的状态
        /// </summary>
        /// <param name="parms"></param>
        private void CompareItems(CompareParms parms)
        {
            IDictionary<object, object> modifyStatus = new Dictionary<object, object>();
            IEnumerator enumerator2 = ((ICollection)parms.Object2).GetEnumerator();
            while (enumerator2.MoveNext())
            {
                var value = enumerator2.Current;
                if (value == null) continue;

                var targetType = value.GetType();
                bool isTransient = (bool)targetType.GetProperties().Where(f => f.Name == "IsTransient").FirstOrDefault()?.GetValue(value);
                if (isTransient)
                {
                    this.AdditionDifference(value, parms);
                }
                else
                {
                    var id = targetType.GetProperties().Where(f => f.Name == "Id" ).FirstOrDefault()?.GetValue(enumerator2.Current);
                    if (!modifyStatus.ContainsKey(id))
                        modifyStatus.Add(id, value);
                    else
                        throw new Exception(string.Format("Duplicate ID modified object detected 【{0}】", id));
                }
            }

            IEnumerator enumerator1 = ((ICollection)parms.Object1).GetEnumerator();
            while (enumerator1.MoveNext())
            {
                var value = enumerator1.Current;
                if (value == null) continue;

                var targetType = value.GetType();
                var id = targetType.GetProperties().Where(f => f.Name == "Id").FirstOrDefault()?.GetValue(value);
                if (modifyStatus.ContainsKey(id))
                {
                    var updValue = modifyStatus[id];
                    this.ModifyDefference(value, updValue, id, parms);
                }
                else
                    this.RemoveDefference(value, id, parms);

            }

        }
        private void AdditionDifference(object newest, CompareParms parms)
        {
            if (parms.Result.ExceededDifferences)
                return;
            Difference difference = new Difference
            {
                Object1 = null,
                Object2 = newest,
                Object1Value = null,
                Object2Value = "_ADD_",
                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                PropertyName = parms.BreadCrumb
            };
            AddDifference(parms.Result, difference);
        }
        private void RemoveDefference(object original, object id, CompareParms parms)
        {
            if (parms.Result.ExceededDifferences)
                return;

            Difference difference = new Difference
            {
                Object1 = original,
                Object2 = null,
                Object1Value = "_DEL_",
                Object2Value = null,
                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                PropertyName = parms.BreadCrumb
            };
            AddDifference(parms.Result, difference);
        }
        private void ModifyDefference(object original, object newest, object id, CompareParms parms)
        {
            //差异对比
            string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, string.Empty, string.Empty, id.ToString());
            CompareParms childParms = new CompareParms
            {
                Result = parms.Result,
                Config = parms.Config,

                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                Object1 = original,
                Object2 = newest,
                BreadCrumb = currentBreadCrumb
            };
            RootComparer.Compare(childParms);
            if (parms.Result.ExceededDifferences)
                return;
        }
    }
}
