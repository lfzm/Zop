

using Orleans.Runtime;

namespace Zop.Repositories
{
    public class RepositoryBuilder
    {
        public RepositoryBuilder(Microsoft.Extensions.DependencyInjection.IServiceCollection service)
        {
            this.Service = service;
        }
        public Microsoft.Extensions.DependencyInjection.IServiceCollection Service { get; }
        /// <summary>
        /// 添加仓储服务
        /// </summary>
        /// <typeparam name="TRepository">存储实现类</typeparam>
        /// <param name="name">存储名称</param>
        /// <returns></returns>
        public RepositoryBuilder AddRepository<TRepository>(string name) where TRepository : class, IRepositoryStorage
        {
            Service.AddScopedNamedService<IRepositoryStorage, TRepository>(name);
            return this;
        }
        /// <summary>
        /// 添加仓储服务
        /// </summary>
        /// <typeparam name="TRepository">存储实现类</typeparam>
        /// <typeparam name="TEntity">存储实体</typeparam>
        /// <returns></returns>
        public RepositoryBuilder AddRepository<TRepository, TEntity>() 
            where TRepository : class, IRepositoryStorage
            where TEntity: class
        {
            this.AddRepository<TRepository>(typeof(TEntity).Name);
            return this;
        }
   
    }
}
