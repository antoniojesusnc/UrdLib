using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Urd.Services.Network
{
    public class NetworkRequestModel
	{
        public string Url { get; private set; }
        public NetworkRequestType RequestType { get; private set; }

        public List<KeyValuePair<string, string>> Headers { get; private set; } =
            new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Accept", "application/json"),
                new KeyValuePair<string, string>("Content-Type", "application/json")
            };
        
		public string PostData { get; private set; }
        public byte[] PutData { get; private set; }
        public bool UseCache { get; private set; }

        private string _responseData;

        public UnityWebRequest.Result Result { get; private set; }
        public string ErrorMessage { get; private set; }


        public NetworkRequestModel(string url) : this(url, NetworkRequestType.Get) { }
        public NetworkRequestModel(string url, NetworkRequestType networkRequestType) : this(url, networkRequestType, null) { }
        public NetworkRequestModel(string url, params KeyValuePair<string, string>[] postDataAsArray) :
            this(url, NetworkRequestType.Post, postData: Newtonsoft.Json.JsonConvert.SerializeObject(postDataAsArray)) { }
        public NetworkRequestModel(string url, object postData): this(url, NetworkRequestType.Post, postData: Newtonsoft.Json.JsonConvert.SerializeObject(postData)) { }
        public NetworkRequestModel(string url, string postData): this(url, NetworkRequestType.Post, postData:postData) { }
        public NetworkRequestModel(string url, byte[] putData) : this(url, NetworkRequestType.Put, putData: putData) { }

        public NetworkRequestModel(string url, NetworkRequestType requestType, string postData = null, byte[] putData = null)
		{
            Contract.Assert(url?.Length > 0, "[NetworkRequestModel] the url is null or empty");

            Url = url;
			RequestType = requestType;
			PostData = postData;
            PutData = putData;
         }

        public void SetResponseData(string responseData)
        {
            Result = UnityWebRequest.Result.Success;
            _responseData = responseData;
        }

        public bool TryGetResponseDataAs<T>(out T data) where T : class
        {
            data = null;
            try
            {
                data = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(_responseData);
                return data != null;
            }
            catch
            {
            }

            return false;
        }
        
        public bool TryPopulateResponseTo<T>(T baseData) where T : class
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings {ObjectCreationHandling = ObjectCreationHandling.Replace};
                Newtonsoft.Json.JsonConvert.PopulateObject(_responseData, baseData, serializerSettings);
                return baseData != null;
            }
            catch
            {
            }

            return false;
        }

        public void SetErrorResponse(string errorMessage, UnityWebRequest.Result errorType, string responseData)
        {
            Result = errorType;
            ErrorMessage = errorMessage;
            _responseData = responseData;
        }

        public void AddHeaders(params KeyValuePair<string,string>[] newHeaders)
        {
            for (int i = 0; i < newHeaders.Length; i++)
            {
                int index = Headers.FindIndex(header => header.Key == newHeaders[i].Key);
                if (index >= 0)
                {
                    Headers[index] = newHeaders[i];
                }
                else
                {
                    Headers.Add(newHeaders[i]);
                }
            }
        }
    }
}
