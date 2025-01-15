using System;
using Firebase;
using Firebase.Crashlytics;
using UnityEngine;

namespace Urd.Services
{
    [Serializable]
    public class ErrorServiceProviderFirebase : IErrorServiceProvider
    {
        private bool _initialized;
        private AnalyticsServiceProviderFirebase _fireBaseProvider;

        public void Init()
        {
            if (!StaticServiceLocator.Get<IAnalyticsService>()
                                     .TryGetProvider<AnalyticsServiceProviderFirebase>(out _fireBaseProvider))
            {
                return;
            }
            
            if (_fireBaseProvider.Status == DependencyStatus.Available)
            {
                InitCrashlytics();
            }
            else
            {
                _fireBaseProvider.OnChangeStatus += OnChangeStatus;
            }
        }

        private void OnChangeStatus()
        {
            if (_fireBaseProvider.Status == DependencyStatus.Available)
            {
                InitCrashlytics();
            }
        }

        private void InitCrashlytics()
        {
            Crashlytics.ReportUncaughtExceptionsAsFatal = true;
            Crashlytics.SetUserId(SystemInfo.deviceUniqueIdentifier);
            _initialized = true;
        }
        
        public void Dispose()
        {
            if (_fireBaseProvider != null)
            {
                _fireBaseProvider.OnChangeStatus -= OnChangeStatus;
            }

        }

        public void LogError(string message)
        {
            if (!_initialized)
            {
                Crashlytics.Log(message);
            }
        }
    }
}