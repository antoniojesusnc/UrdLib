using System;
using Newtonsoft.Json;

namespace Urd.Error
{
    public class ErrorNetworkModel : IErrorNetworkModel
    {
        [JsonProperty("detail")] public string Details { get; private set; }

        public ErrorNetworkModel(string details)
        {
            Details = details;
        }

        public void Dispose()
        {
        }

        public override string ToString()
        {
            return Details;
        }
    }
}