using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Zop.Repositories.ChangeDetector;


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
        protected Task<IChangeManager> GetChangeManagerAsync(TEntity newEntity)
        {
            if (ChangeDetector == null)
                throw new ZopException("Change Detector cannot be empty");

            var id = (TPrimaryKey)typeof(TEntity).GetProperties().Where(f => f.Name == "Id")
                .FirstOrDefault()?.GetValue(newEntity);
            string key = typeof(TEntity).FullName + id;
            var oldEntity = this.cache.GetOrCreate(key, e =>
            {
                return this.GetAsync(id).Result;
            });

            //验证版本
            int newVersionNo = 0;
            if (typeof(AggregateConcurrencySafe<TPrimaryKey>).IsInstanceOfType(newEntity))
            {
                int oldVersionNo = this.GetVersionNo(oldEntity);
                newVersionNo = this.GetVersionNo(newEntity);
                //差异的对比前，版本号判断
                if (oldVersionNo != (newVersionNo - 1))
                    throw new RepositoryDataException("实体修改失败，版本号不一致");
            }
            var cm = this.ChangeDetector.DetectChanges(newEntity, oldEntity, newVersionNo);
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
            //快照存储
            if (e != null)
                this.SnapshotStorage(e, id);
            return e;
        }
        public async Task<object> WriteAsync(object id, object entity)
        {
            if (typeof(TEntity) != (entity.GetType()))
            {
                throw new RepositoryDataException("WriteAsync：entity is not the same type as TEntity");
            }
            //判断是否是领域实体，如果是领域实体，就判断是新增还在修改
            TEntity e = (TEntity)entity;
            //如果Grain的PrimaryKeyId和实体的唯一标示不同，则添加
            var primaryKey = e.GetPrimaryKey();
            if (primaryKey != id)
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
                this.SnapshotStorage(e, primaryKey);
            }
            return e;
        }

        /// <summary>
        /// 设置线程安全锁，版本号
        /// </summary>
        /// <param name="entity">实体</param>
        public void SetVersionNo(object entity)
        {
            if (entity == null) return;
            //判断是否需要设置版本号
            if (typeof(AggregateConcurrencySafe<TPrimaryKey>).IsInstanceOfType(entity))
            {
                ((AggregateConcurrencySafe<TPrimaryKey>)entity).VersionNo++;
            }
        }
        /// <summary>
        /// 获取版本号
        /// </summary>
        /// <param name="obj">实体</param>
        /// <returns></returns>
        public int GetVersionNo(object obj)
        {
            return ((AggregateConcurrencySafe<TPrimaryKey>)obj).VersionNo;
        }
        /// <summary>
        /// 快照存储
        /// </summary>
        /// <param name="entity">实体对象</param>
        private void SnapshotStorage(object entity, object id)
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
