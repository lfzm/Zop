
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using Zop.Domain.Entities;
using Zop.Domain.Values;

namespace Zop.Repositories.ChangeDetector
{
    public class ChangeManagerFactory : IChangeManagerFactory
    {
        public IChangeManager Create(EntityChange entityChange, ComparisonResult comparisonResult)
        {
            var changeManager = new ChangeManager(entityChange);
            if (comparisonResult.AreEqual)
                return changeManager;

            //根据实体与快照的差异进行分析实体的变动
            foreach (var difference in comparisonResult.Differences)
            {
                EntityType type = this.GetEntityType(difference);

                //非基础类检查是否为添加或者删除变动类型
                if (type != EntityType.Basis)
                {
                    if (this.RemoveDiff(entityChange, difference))
                        continue;
                    if (this.AdditionDiff(entityChange, difference, type))
                        continue;
                }
                this.ModifyDiff(entityChange, difference);
            }
            return changeManager;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="difference"></param>
        /// <returns></returns>
        private bool AdditionDiff(EntityChange entityChange, Difference difference, EntityType type)
        {
            if (difference.Object2 == null)
                return false;
            if (difference.Object1 != null)
                return false;

            //新增数据
            EntityDifference diff = new EntityDifference(difference.Object2.GetType().FullName, EntityChangeType.Addition, difference.ParentPropertyName);
            var key = difference.Object2.GetHashCode();
            entityChange.ChangeDifference.Add(key, diff);
            return true;
        }

        private bool RemoveDiff(EntityChange entityChange, Difference difference)
        {
            if (difference.Object2 != null)
                return false;
            if (difference.Object1 == null)
                return false;

            var diff = new EntityDifference(difference.Object1.GetType().FullName, EntityChangeType.Remove, difference.ParentPropertyName);
            diff.OldEntity = difference.Object1;
            entityChange.DeleteEntry.Add(diff);
            return true;
        }

        private void ModifyDiff(EntityChange entityChange, Difference difference)
        {
            object parentObj = difference.ParentObject1 != null ? difference.ParentObject1 : difference.ParentObject2;
            //生成哈希Key
            int key = parentObj.GetHashCode();
            entityChange.ChangeDifference.TryGetValue(key, out EntityDifference diff);
            if (diff == null)
            {
                diff = new EntityDifference(parentObj.GetType().FullName, EntityChangeType.Modify, difference.ParentPropertyName);
                entityChange.ChangeDifference.Add(key, diff);
            }

            string name = difference.PropertyName.Substring(difference.PropertyName.LastIndexOf(".") + 1);
            EntityChangePropertys propertys = new EntityChangePropertys(name, difference.Object1, difference.Object2);
            diff.ChangePropertys.Add(propertys);
        }

        /// <summary>
        /// 判断是否为子类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private EntityType GetEntityType(Difference difference)
        {
            object obj = difference.Object1 != null ? difference.Object1 : difference.Object2;

            if (typeof(IValueObject).IsAssignableFrom(obj.GetType()))
                return EntityType.ValueObject;
            if (typeof(IEntity).IsAssignableFrom(obj.GetType()))
                return EntityType.Entity;
            else
                return EntityType.Basis;

        }

    }
}
