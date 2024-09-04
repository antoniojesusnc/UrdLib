using System;
using Newtonsoft.Json;

namespace Urd.Ads
{
    [Serializable]
    public class AdMobBannerError
    {
        public const int INTERNAL_ERROR = 0;
        public const int INVALID_REQUEST = 1;
        public const int NETWORK_ERROR = 2;
        public const int CODE_NO_FILL = 3;
        
        [JsonProperty("Code")]
        public int Code { get; private set; } = -1;
        [JsonProperty("Message")]
        public string Message { get; private set; }
        [JsonProperty("Domain")]
        public string Domain { get; private set; }
        [JsonProperty("Cause")]
        public string Cause { get; private set; }
        [JsonProperty("Response Info")]
        public AdMobBannerErrorResponse Response { get; private set; }
        
        public bool IsSuccess => Code <= 0;

        public void SetAsError()
        {
            Code = 1;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    
    [Serializable]
    public class AdMobBannerErrorResponse
    {
        [JsonProperty("ResponseID")]
        public string ResponseID { get; private set; }
        [JsonProperty("Mediation Adapter Class Name")]
        public string MediationAdapterClassName { get; private set; }
    }
}