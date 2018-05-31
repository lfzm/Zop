using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.OrleansClient
{
    /// <summary>
    /// Orleans 客户端
    /// </summary>
    public interface IOrleansClient 
    {
        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerKey;

        TGrainInterface GetGrain<TGrainInterface>(string primaryKey, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithStringKey;

        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithGuidCompoundKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, AccessTokenType accessType = AccessTokenType.Default, string grainClassNamePrefix = null) where TGrainInterface : IGrainWithIntegerCompoundKey;
    }
}
