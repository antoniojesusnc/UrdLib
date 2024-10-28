using System;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class ErrorServiceProviderFirebase : IErrorServiceProvider
    {
        public void Init()
        {
            /*
            Firebase.FirebaseApp.CheckAndFixDependicesAsync().ContinueWith(
                task =>
                {
                    var dependencyStatus = task.Result;
                    if (dependencyStatus == Firebase.DependencyStatus.Available)
                    {
                        Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
                        CrashLytics.ReportUncaughtExceptionAsFatal = true;
                    }
                    else
                    {
                        Debug.LogWarning("ErrorServiceProviderFirebase not initialized");
                    }
                });
            */
        }
    }
}