using System;
using System.Collections.Generic;
using System.Text.Json;

namespace Urd.Sockets
{
    public interface INetworkSocketAdapter
    {
        bool IsConnected { get; }
        void Init();
        void Connect(string url);

        void EmitData(string reference, Action<JsonElement> callback,
            params KeyValuePair<string, string>[] values);
        void SubscribeToReferenceChanges(string reference, Action<JsonElement> callback);
        void SubscribeToReferenceChanges<T>(string reference, Action<T> callback) where T : class;
        void UnsubscribeToReferenceChanges(string reference);
    }
}