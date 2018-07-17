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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Zop.Repositories
{
    public abstract class Repository<TEntity, TPrimaryKey> :
        IRepositoryStorage, IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity
    {
        private readonly IMemoryCache cache;
        /// <summary>
        /// 变动探测器
        /// </summary>
        protected readonly IChangeDetector ChangeDetector;
        public Repository(IServiceProvider _serviceProvider)
        {
            this.ChangeDetector = _serviceProvider?.GetService<IChangeDetector>();
            this.cache = _serviceProvider?.GetService<IMemoryCache>(); ;
        }
        /// <summary>
        /// 获取变动管理器
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected Task<IChangeManager> GetChangeManagerAsync(TEntity entity)
        {
            if (ChangeDetector == null)
                throw new ZopException("Change Detector cannot be empty");

            var id = (TPrimaryKey)typeof(TEntity).GetProperties().Where(f => f.Name == "Id")
                .FirstOrDefault()?.GetValue(entity);
            string key = typeof(TEntity).FullName + id;
            var oleEntity = this.cache.GetOrCreate(key, e =>
          {
              return this.GetAsync(id).Result;
          });
            var cm = this.ChangeDetector.DetectChanges(entity, oleEntity);
            return Task.FromResult(cm);
        }

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
            //存储快照
            if (e != null)
                this.SetEntitySnapshot(e, id);
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
                this.SetVersionNo(e);
                e = await this.UpdateAsync(e);
            }
            //存储快照
            if (e != null)
            {
                var id = (TPrimaryKey)typeof(TEntity).GetProperties() .Where(f => f.Name == "Id")
                    .FirstOrDefault()?.GetValue(entity);
                this.SetEntitySnapshot(e, id);
            }
            return e;
        }

        /// <summary>
        /// 设置线程安全锁，版本号
        /// </summary>
        /// <param name="entity">实体</param>
        private void SetVersionNo(object entity)
        {
            if (entity == null) return;
            //判断是否需要设置版本号
            if (typeof(AggregateConcurrencySafe<TPrimaryKey>).IsInstanceOfType(entity))
            {
                ((AggregateConcurrencySafe<TPrimaryKey>)entity).VersionNo++;
            }
        }
        /// <summary>
        /// 设置快照
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void SetEntitySnapshot(object entity,object id)
        {
            if (entity == null) return;
            //变动探测器为空无需设置快照
            if (ChangeDetector != null)
            {
                this.cache.Set(typeof(TEntity).FullName + id, ((IEntity)entity).Clone<TEntity>());
            }
        }
    }
}
