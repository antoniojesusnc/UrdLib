using System;
using UnityEngine;
using Urd.Error;
using Urd.Services.Network;
using Urd.Sockets;

namespace Urd.Services
{
    public interface INetworkService : IBaseService
    {
        NetworkEnvironment Environment { get; }
        INetworkSocketAdapter SocketAdapter { get; }
        void Request(NetworkRequestModel networkRequestModel, Action<NetworkRequestModel> onRequestHttpFinishedSuccess, Action<ErrorModel> onRequestHttpFinishedFailed);
        void LoadTexture(string imageUrl, Action<Sprite> onLoadTextureSuccess, Action<ErrorModel> onLoadTextureFailed);
    }
}