using System;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class AnalyticsServiceProviderEditor : IAnalyticsServiceProvider
    {
        public void Init()
        {
            if (Application.isEditor)
            {
                Debug.Log("[AnalyticsServiceEditor] Init");
            }
        }

        public void LogEvent(string eventKey, string eventValue)
        {
            if (Application.isEditor)
            {
                Debug.Log($"[AnalyticsServiceEditor]: {eventKey}:{eventValue}");
            }
        }
    }
}