using System;
using System.Collections.Generic;
using System.Text.Json;
using Urd.Sockets;

namespace Urd.DTO
{
    [Serializable]
    public class DummyNetworkSocketAdapter : INetworkSocketAdapter
    {
        public bool IsConnected { get; }
        public void Init(){ }

        public void Connect(string url) { }
        public void EmitData(string reference, Action<JsonElement> callback, params KeyValuePair<string, string>[] values) {}

        public void SubscribeToReferenceChanges(string reference, Action<JsonElement> callback) {}

        public void SubscribeToReferenceChanges<T>(string reference, Action<T> callback) where T : class {}

        public void UnsubscribeToReferenceChanges(string reference) {}
    }
}