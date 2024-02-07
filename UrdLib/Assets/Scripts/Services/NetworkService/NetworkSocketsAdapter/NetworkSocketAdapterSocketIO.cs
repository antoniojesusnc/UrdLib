using System;
using System.Collections.Generic;
using System.Text.Json;
using PimDeWitte.UnityMainThreadDispatcher;
using SocketIOClient;
using SocketIOClient.Newtonsoft.Json;
using UnityEngine;
using Urd.Sockets;

namespace Frens.Sockets
{
    [Serializable]
    public class NetworkSocketAdapterSocketIO : INetworkSocketAdapter
    {
        public SocketIOUnity _socket;

        public bool IsConnected => _socket?.Connected == true;

        public void Init()
        {
            _socket = null;
        }
        
        public void Connect(string url)
        {
            var socketIOOptions = new SocketIOOptions();
            var parameters = AddParameters();
            socketIOOptions.Auth = parameters;

            if (_socket?.Connected == true)
            {
                return;
            }
            _socket = new SocketIOUnity(url, socketIOOptions);
            _socket.OnConnected += OnConnect;
            _socket.OnDisconnected += OnDisconnected;
            _socket.OnError += OnError;
            _socket.OnReconnectAttempt += OnReconnectAttempt;

            _socket.JsonSerializer = new NewtonsoftJsonSerializer();
            
            _socket.On("connect_result", OnConnectResult);
            _socket.Connect();
            Debug.Log($"[NetworkSocket] Connect");
        }

        private void OnReconnectAttempt(object sender, int e)
        {
            Debug.Log($"OnReconnectAttempt. Sender{sender}, error: {e}");
        }

        public void SubscribeToReferenceChanges(string reference, Action<JsonElement> callback)
        {
            Debug.Log($"[NetworkSocket] SubscribeToReferenceChanges, reference:{reference}");
            _socket.On(reference, response =>
                           UnityMainThreadDispatcher.Instance().Enqueue(
                               () => callback?.Invoke(response.GetValue())));
        }

        public void SubscribeToReferenceChanges<T>(string reference, Action<T> callback) where T : class
        {
            Debug.Log($"[NetworkSocket] SubscribeToReferenceChangesT, reference:{reference}");
            _socket.On(reference, response => callback?.Invoke(response.GetValue<T>()));
        }

        public void UnsubscribeToReferenceChanges(string reference)
        {
            Debug.Log($"[NetworkSocket] UnsubscribeToReferenceChanges, reference:{reference}");
            _socket.Off(reference);
        }

        private void OnConnectResult(SocketIOResponse socketIOResponse)
        {
            Debug.Log($"OnConnectResult: {socketIOResponse}");
        }

        private void OnError(object sender, string e)
        {
            Debug.Log($"OnError. Sender{sender}, error: {e}");
        }

        private void OnDisconnected(object sender, string e)
        {
            Debug.Log($"OnDisconnected. Sender{sender}, error: {e}");
        }

        private void OnConnect(object sender, EventArgs events)
        {
            Debug.Log($"OnConnect. Sender{sender}, error: {events}");
            
        }

        public void EmitData(string reference, Action<JsonElement> callback, params KeyValuePair<string, string>[] values)
        {
            Debug.Log($"[NetworkSocket] EmitData, reference:{reference}, values:{values}");
            _socket.Emit(reference, (socketIOResponse) => OnEmitData(socketIOResponse, callback), AddParameters(values));
        }
        
        private void OnEmitData(SocketIOResponse response, Action<JsonElement> callback)
        {
            //TODO handle wrong connection
            Debug.Log($"[NetworkSocket] OnEmitData, reference:{response.GetValue()}");
            UnityMainThreadDispatcher.Instance().Enqueue(
                () => callback?.Invoke(response.GetValue()));
        }

        protected virtual Dictionary<string,string> AddParameters(params KeyValuePair<string,string>[] values)
        {
            var parameters = new Dictionary<string, string>();

            if (values?.Length > 0)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    parameters.Add(values[i].Key, values[i].Value);
                }
            }
            return parameters;
        }
    }
}
