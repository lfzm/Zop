using Orleans.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Zop.Domain.Entities;
using Orleans;
using Orleans.Runtime;
using Microsoft.Extensions.Logging;

namespace Zop.Repositories
{
    public class RepositoryStorage : IGrainStorage
    {
        private IServiceProvider ServiceProvider;
        private readonly ILogger Logger;
        public RepositoryStorage(IServiceProvider serviceProvider, ILogger<RepositoryStorage> logger)
        {
            ServiceProvider = serviceProvider;
            this.Logger = logger;
        }

        /// <summary>
        /// RepositoryStorage 默认名称
        /// </summary>
        public const string DefaultName = "DefaultRepositoryStorage";
        public Task ClearStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            this.GetRepository(grainState).ClearAsync(grainState.State);
            return Task.CompletedTask;
        }
        public async Task ReadStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            object id = grainReference.GetPrimaryKeyObject();
            grainState.State = await this.GetRepository(grainState).ReadAsync(id);
            this.SetETag(grainState);
        }
        public async Task WriteStateAsync(string grainType, GrainReference grainReference, IGrainState grainState)
        {
            try
            {
                if (grainState == null)
                    throw new RepositoryDataException("修改的状态对象不能为空");
                grainState.State = await this.GetRepository(grainState).WriteAsync(grainState.State);
            }
            catch (Exception ex)
            {
                var data = new
                {
                    data = grainState,
                    name = grainState.GetType()
                };
                this.Logger.LogError(ex, data.ToJsonString());
                throw new RepositoryDataException(ex.Message);
            }
        }
        private void SetETag(IGrainState grainState)
        {
            if (grainState.State == null)
                return;
            if (typeof(IConcurrencySafe).IsAssignableFrom(grainState.State.GetType()))
            {
                grainState.ETag = ((IConcurrencySafe)grainState.State).VersionNo.ToString();
            }

        }
      
        private IRepositoryStorage GetRepository(IGrainState grainState)
        {
            var Repository = ServiceProvider.GetServiceByName<IRepositoryStorage>(grainState.State.GetType().Name);
            if (Repository == null)
                throw new RepositoryDataException(string.Format("{0} State Repository Unrealized", grainState.State.GetType().Name));
            return Repository;
        }
    }
}
