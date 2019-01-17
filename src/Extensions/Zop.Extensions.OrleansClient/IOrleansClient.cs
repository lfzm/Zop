using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.Extensions.OrleansClient
{
    /// <summary>
    /// Orleans 客户端
    /// </summary>
    public interface IOrleansClient 
    {
        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey;

        TGrainInterface GetGrain<TGrainInterface>(string primaryKey,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey;

        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension,string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey;
    }
}
