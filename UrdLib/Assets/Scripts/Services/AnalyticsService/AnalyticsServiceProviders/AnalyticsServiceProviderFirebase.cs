using System;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class AnalyticsServiceProviderFirebase : IAnalyticsServiceProvider
    {
        public void Init()
        {
            if (!Application.isEditor)
            {
                Firebase.Analytics.FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
                Firebase.Analytics.FirebaseAnalytics.LogEvent(Firebase.Analytics.FirebaseAnalytics.EventLogin);
            }
        }

        public void LogEvent(string eventKey, string eventValue)
        {
            if (!Application.isEditor)
            {
                SetUserProperties();
                
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventKey,
                                                              Firebase.Analytics.FirebaseAnalytics.ParameterValue,
                                                              eventValue);
            }
        }

        protected virtual void SetUserProperties()
        {
            
        }
    }
}