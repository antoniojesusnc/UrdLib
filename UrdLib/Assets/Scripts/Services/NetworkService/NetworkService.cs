using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Urd.Error;
using Urd.Services.Network;
using Urd.Sockets;

namespace Urd.Services
{
    [Serializable]
    public class NetworkService : BaseService, INetworkService
    {
        [field: SerializeField]
        public NetworkEnvironment Environment { get; private set; }
            
        [field: SerializeReference, SubclassSelector]
        public INetworkSocketAdapter SocketAdapter { get; private set; }

        public override int LoadPriority => 30;

        private ICoroutineService _coroutineService;

        public override void Init()
        {
            base.Init();

            _coroutineService = StaticServiceLocator.Get<ICoroutineService>();
            
            SocketAdapter.Init();
        }

        public void Request(NetworkRequestModel networkRequestModel,
            Action<NetworkRequestModel> onRequestHttpFinishedSuccess, Action<ErrorModel> onRequestHttpFinishedFailed)
        {
            _coroutineService.StartCoroutine(RequestCo(networkRequestModel, onRequestHttpFinishedSuccess,
                                                       onRequestHttpFinishedFailed));
        }

        private IEnumerator RequestCo(NetworkRequestModel networkRequestModel,
            Action<NetworkRequestModel> onRequestHttpFinishedSuccess, Action<ErrorModel> onRequestHttpFinishedFailed)
        {
            var unityWebRequest = GetWebRequest(networkRequestModel);
            for (int i = 0; i < networkRequestModel.Headers.Count; i++)
            {
                unityWebRequest.SetRequestHeader(networkRequestModel.Headers[i].Key,
                                                 networkRequestModel.Headers[i].Value);
            }

            if (networkRequestModel.RequestType == NetworkRequestType.Post ||
                networkRequestModel.RequestType == NetworkRequestType.Put)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(networkRequestModel.PostData);
                var uploadHandler = new UploadHandlerRaw(bytes);
                uploadHandler.contentType = "application/json";
                unityWebRequest.uploadHandler = uploadHandler;
                /* */
                if (unityWebRequest.uploadHandler != null)
                {
                    unityWebRequest.uploadHandler.contentType = "application/json";
                }
            }
            
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                networkRequestModel.SetResponseData(unityWebRequest.downloadHandler.text);
                onRequestHttpFinishedSuccess?.Invoke(networkRequestModel);
            }
            else
            {
                networkRequestModel.SetErrorResponse(unityWebRequest.error, unityWebRequest.result,
                                                     unityWebRequest?.downloadHandler?.text);
                var error = new ErrorModel(unityWebRequest.error, unityWebRequest.responseCode, unityWebRequest.result);
                if (networkRequestModel.TryGetResponseDataAs(out ErrorNetworkModel errorNetworkModel))
                {
                    error.SetNetworkModel(errorNetworkModel);
                }

                Debug.LogWarning(error.ToString());
                onRequestHttpFinishedFailed?.Invoke(error);
            }
        }
        
        public void LoadTexture(string imageUrl, Action<Sprite> onLoadTextureSuccess, Action<ErrorModel> onLoadTextureFailed)
        {
            _coroutineService.StartCoroutine(LoadTextureCo(imageUrl, onLoadTextureSuccess, onLoadTextureFailed));
        }

        private IEnumerator LoadTextureCo(string imageUrl, Action<Sprite> onLoadTextureSuccess, Action<ErrorModel> onLoadTextureFailed)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl);
            yield return webRequest.SendWebRequest();
            
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                var texture2D = DownloadHandlerTexture.GetContent(webRequest);
                Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), 
                                              new Vector2(texture2D.width *0.5f , texture2D.height *0.5f));

                onLoadTextureSuccess?.Invoke(sprite);
            }
            else
            {
                onLoadTextureFailed?.Invoke(new ErrorModel($"Cannot donload texture from: {imageUrl}", ErrorCode.Error_404_Not_Found));
            }
        }

        private UnityWebRequest GetWebRequest(NetworkRequestModel networkRequestModel)
        {
            switch (networkRequestModel.RequestType)
            {
                case NetworkRequestType.Get:
                    return UnityWebRequest.Get(networkRequestModel.Url);
                case NetworkRequestType.Post:
                    return UnityWebRequest.Post(networkRequestModel.Url, networkRequestModel.PostData);
                case NetworkRequestType.Put:
                    return UnityWebRequest.Put(networkRequestModel.Url, networkRequestModel.PutData);
                case NetworkRequestType.Head:
                    return UnityWebRequest.Head(networkRequestModel.Url);
                default: return UnityWebRequest.Get(networkRequestModel.Url);
            }
        }
    }
}