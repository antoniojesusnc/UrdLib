using System;
using Firebase;
using Firebase.Analytics;
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
            if (Application.isEditor)
            {
                return;
            }

            Status = DependencyStatus.UnavailableUpdating;
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(
                task =>
                {
                    var dependencyStatus = task.Result;
                    Status = dependencyStatus;
                    if (dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                        Firebase.Analytics.FirebaseAnalytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
                        Firebase.Analytics.FirebaseAnalytics.LogEvent(
                            Firebase.Analytics.FirebaseAnalytics.EventLogin);
                    }
                    OnChangeStatus?.Invoke();
                });
        }

        public void LogEvent(string eventKey, string eventValue)
        {
            if (Application.isEditor || Status != DependencyStatus.Available)
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