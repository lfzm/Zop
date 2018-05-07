using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Zop.OrleansClient
{
    /// <summary>
    /// Orleans 客户端
    /// </summary>
    public interface IOrleansClient : IGrainFactory
    {
        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, AccessTokenType accessType) where TGrainInterface : IGrainWithGuidKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, AccessTokenType accessType) where TGrainInterface : IGrainWithIntegerKey;

        TGrainInterface GetGrain<TGrainInterface>(string primaryKey, AccessTokenType accessType) where TGrainInterface : IGrainWithStringKey;

        TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, string keyExtension, AccessTokenType accessType) where TGrainInterface : IGrainWithGuidCompoundKey;

        TGrainInterface GetGrain<TGrainInterface>(long primaryKey, string keyExtension, AccessTokenType accessType) where TGrainInterface : IGrainWithIntegerCompoundKey;
    }
}
