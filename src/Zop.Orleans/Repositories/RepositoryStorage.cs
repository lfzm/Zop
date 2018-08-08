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
            grainState.State = null;
            this.SetETag(grainState);
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

                if (grainState.ETag.Equals("0"))
                    grainState.State = await this.GetRepository(grainState).AddAsync(grainState.State);
                else
                    grainState.State = await this.GetRepository(grainState).ModifyAsync(grainState.State);
                this.SetETag(grainState);
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
                grainState.ETag = "0";

            else
                grainState.ETag = "1";

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
