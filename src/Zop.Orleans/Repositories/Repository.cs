using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Zop.Repositories;
using Zop.Repositories.ChangeDetector;

namespace Zop.Repositories
{
    public abstract class Repository<TEntity, TPrimaryKey> : IRepositoryStorage,IRepository<TEntity, TPrimaryKey> where TEntity : class,IEntity
    {
        public Repository()
        {

        }
        public Repository(IChangeDetector _changeDetector)
        {
            this.ChangeDetector = _changeDetector;
        }
        /// <summary>
        /// 变动探测器
        /// </summary>
        protected readonly IChangeDetector ChangeDetector;
        /// <summary>
        /// 获取变动管理器
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected async Task<IChangeManager> GetChangeManagerAsync(TEntity entity)
        {
            if (ChangeDetector == null)
                throw new ZopException("Change Detector cannot be empty");
            if (this.EntitySnapshot == null)
            {
                var id = (TPrimaryKey)typeof(TEntity).GetProperties().Where(f => f.Name == "Id" && f.PropertyType.IsValueType)
                    .FirstOrDefault()?.GetValue(entity);
                this.EntitySnapshot = await this.GetAsync(id);
            }
            return this.ChangeDetector.DetectChanges(entity, this.EntitySnapshot);
        }
        /// <summary>
        /// 实体快照
        /// </summary>
        protected TEntity EntitySnapshot { get;  set; }
        /// <summary>
        /// 获取仓储
        /// </summary>
        /// <param name="id">标识ID</param>
        /// <returns></returns>
        public abstract Task<TEntity> GetAsync(TPrimaryKey id);
        /// <summary>
        /// 修改存储
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public abstract Task<TEntity> UpdateAsync(TEntity entity);
        /// <summary>
        /// 插入存储
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public abstract Task<TEntity> InsertAsync(TEntity entity);
        /// <summary>
        /// 删除存储
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public abstract Task DeleteAsync(TEntity entity);
        public Task ClearAsync(object entity)
        {
            if (entity.GetType() != typeof(TEntity))
            {
                throw new RepositoryDataException("ClearAsync：entity is not the same type as TEntity");
            }
            return this.DeleteAsync((TEntity)entity);
        }
        public async Task<object> ReadAsync(object id)
        {
            if (id.GetType() != typeof(TPrimaryKey))
            {
                if (id.GetType() == typeof(long) &&
                    typeof(TPrimaryKey) == typeof(int))
                {
                    id = Convert.ToInt32(id); //转换成32 Int
                }
                else
                    return null;
            }
            var e = await this.GetAsync((TPrimaryKey)id);
            //初始化实体
            if (e != null)
            {
                this.InitEntity(e);
                this.SetEntitySnapshot(e);
            }
            return e;
        }
        public async Task<object> WriteAsync(object entity)
        {
            if (typeof(TEntity) != (entity.GetType()))
            {
                throw new RepositoryDataException("WriteAsync：entity is not the same type as TEntity");
            }
            //判断是否是领域实体，如果是领域实体，就判断是新增还在修改
            TEntity e = (TEntity)entity;
            if (e.IsTransient)
            {
                //插入数据
                e = await this.InsertAsync(e);
            }
            else
            {
                //修改数据
                e = await this.UpdateAsync(e);
            }
            //初始化实体
            if (e != null)
            {
                this.InitEntity(e);
                this.SetEntitySnapshot(e);
            }
            return e;
        }
        /// <summary>
        /// 实体初始化
        /// 1、清除属性修改记录器
        /// 2、设置版本号
        /// </summary>
        /// <param name="entity">实体</param>
        private void InitEntity(object entity)
        {
            if (entity == null) return;
            try
            {
                //判断是否需要设置版本号
                if (typeof(AggregateConcurrencySafe<TPrimaryKey>).IsInstanceOfType(entity))
                {
                    //设置版本号
                    ((AggregateConcurrencySafe<TPrimaryKey>)entity).VersionNo++;
                }
                //循环清除属性修改记录器
                foreach (var info in entity.GetType().GetProperties())
                {
                    var value = info.GetValue(entity);
                    if (value == null)
                        continue;
                    if (typeof(IEntity).IsAssignableFrom(info.PropertyType))
                    {
                        this.InitEntity(value);
                    }
                    else if (typeof(IList).IsAssignableFrom(info.PropertyType))
                    {
                        if (typeof(IEntity).IsAssignableFrom(info.PropertyType.GenericTypeArguments[0]))
                        {
                            foreach (var item in ((IList)value))
                            {
                                this.InitEntity(item);
                            }
                        }
                        //清除修改的属性
                        else if (info.Name == "ChangedProperty")
                        {
                            ((IList)value).Clear();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 设置快照
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void SetEntitySnapshot(object entity)
        {
            if (entity == null) return;
            //变动探测器为空无需设置快照
            if (ChangeDetector != null)
                this.EntitySnapshot = ((IEntity)entity).Clone<TEntity>();
        }
    }
}
