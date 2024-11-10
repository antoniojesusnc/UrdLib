using System;
using Firebase.Crashlytics;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class ErrorServiceProviderFirebase : IErrorServiceProvider
    {
        public void Init()
        {
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(
                task =>
                {
                    var dependencyStatus = task.Result;
                    if (dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                        Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                        Crashlytics.ReportUncaughtExceptionsAsFatal = true;
                    }
                    else
                    {
                        Debug.LogWarning("ErrorServiceProviderFirebase not initialized");
                    }
                });
        }
    }
}