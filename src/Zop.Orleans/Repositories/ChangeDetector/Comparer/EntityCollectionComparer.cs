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
    public class EntityCollectionComparer : CollectionComparer
    {
        public EntityCollectionComparer(RootComparer rootComparer) : base(rootComparer)
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
            IDictionary<object, object> object1s = this.GetObject1Values(parms);
            IDictionary<object, object> object2s = this.GetObject2Values(parms);
            //获取旧数据和新数据的交集
            var intersect = object1s.Keys.Intersect(object2s.Keys).ToList();


            //标记修改差异
            foreach (var key in intersect)
            {
                var object1 = object1s[key];
                var object2 = object2s[key];
                this.ModifyDefference(object1, object2, key, parms);
            }

            //标记删除差异
            foreach (var key in object1s.Keys)
            {
                if (intersect.Contains(key))
                    continue;
                var object1 = object1s[key];
                this.RemoveDefference(object1, key, parms);
            }

            //标记新增差异
            foreach (var key in object2s.Keys)
            {
                if (intersect.Contains(key))
                    continue;

                var object2 = object2s[key];
                this.AdditionDifference(object2,  parms);
            }
        }

        private IDictionary<object, object> GetObject1Values(CompareParms parms)
        {
            IDictionary<object, object> values = new Dictionary<object, object>();
            var objects = ((ICollection)parms.Object1).GetEnumerator();
            while (objects.MoveNext())
            {
                var value = objects.Current;
                var primaryKey = value.GetType().GetMethod("GetPrimaryKey").Invoke(value, new object[0]);
                if (primaryKey == null)
                    throw new Exception($"The primary key of the {value.GetType().FullName} of the snapshot is empty");

                values.Add(primaryKey, value);
            }
            return values;
        }

        private IDictionary<object, object> GetObject2Values(CompareParms parms)
        {
            IDictionary<object, object> values = new Dictionary<object, object>();
            var objects = ((ICollection)parms.Object2).GetEnumerator();
            while (objects.MoveNext())
            {
                var value = objects.Current;
                var primaryKey = value.GetType().GetMethod("GetPrimaryKey").Invoke(value, new object[0]);
                //唯一标示为空，标示为添加数据
                if (primaryKey == null)
                {
                    this.AdditionDifference(value, parms);
                    continue;
                }
                else
                {
                    values.Add(primaryKey, value);
                }
            }
            return values;
        }


        private void AdditionDifference(object object2, CompareParms parms)
        {
            if (parms.Result.ExceededDifferences)
                return;
            Difference difference = new Difference
            {
                Object1 = null,
                Object2 = object2,
                Object1Value = null,
                Object2Value = "_ADD_",
                ParentObject1 = parms.ParentObject1,
                ParentObject2 = parms.ParentObject1,
                PropertyName = parms.BreadCrumb
            };
            AddDifference(parms.Result, difference);

        }
        private void RemoveDefference(object object1, object id, CompareParms parms)
        {
            if (parms.Result.ExceededDifferences)
                return;

            Difference difference = new Difference
            {
                Object1 = object1,
                Object2 = null,
                Object1Value = "_DEL_",
                Object2Value = null,
                ParentObject1 = parms.ParentObject1,
                ParentObject2 = parms.ParentObject1,
                PropertyName = parms.BreadCrumb
            };
            AddDifference(parms.Result, difference);
        }
        private void ModifyDefference(object object1, object object2, object id, CompareParms parms)
        {
            //差异对比
            string currentBreadCrumb = AddBreadCrumb(parms.Config, parms.BreadCrumb, string.Empty, string.Empty, id.ToString());
            CompareParms childParms = new CompareParms
            {
                Result = parms.Result,
                Config = parms.Config,

                ParentObject1 = parms.Object1,
                ParentObject2 = parms.Object2,
                Object1 = object1,
                Object2 = object2,
                BreadCrumb = currentBreadCrumb
            };
            RootComparer.Compare(childParms);
            if (parms.Result.ExceededDifferences)
                return;
        }
    }
}
