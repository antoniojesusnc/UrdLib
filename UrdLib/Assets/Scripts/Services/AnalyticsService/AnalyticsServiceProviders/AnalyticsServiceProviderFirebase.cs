using System;
using Firebase;
using Firebase.Extensions;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class AnalyticsServiceProviderFirebase : IAnalyticsServiceProvider
    {
        public DependencyStatus Status { get; private set; } = DependencyStatus.UnavilableMissing;
        public event Action OnChangeStatus;

        public void Init()
        {
            Status = DependencyStatus.UnavailableUpdating;
            //Debug.Log("[AnalyticsServiceProviderFirebase] CheckAndFixDependenciesAsync");
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(
                task =>
                {
                    var dependencyStatus = task.Result;
                    Status = dependencyStatus;
                    //Debug.Log("[AnalyticsServiceProviderFirebase] Init:" + Status);
                    if (dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                        Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                        if (!Application.isEditor)
                        {
                            Firebase.Analytics.FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
                            Firebase.Analytics.FirebaseAnalytics.LogEvent(
                                Firebase.Analytics.FirebaseAnalytics.EventLogin);
                        }
                    }

                    OnChangeStatus?.Invoke();
                });
        }

        public void LogEvent(string eventKey, string eventValue)
        {
            if (Status != DependencyStatus.Available || Application.isEditor)
            {
                return;
            }

            SetUserProperties();
            
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventKey,
                                                          Firebase.Analytics.FirebaseAnalytics.ParameterValue,
                                                          eventValue);
        }

        protected virtual void SetUserProperties()
        {

        }
    }
}