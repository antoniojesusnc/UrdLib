#if FIREBASE
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
                //Debug.Log("[ErrorServiceProviderFirebase] Cannot Init");
                return;
            }
            
            if (_fireBaseProvider.Status == DependencyStatus.Available)
            {
                InitCrashlytics();
            }
            else
            {
                //Debug.Log("[ErrorServiceProviderFirebase] Subscribe");
                _fireBaseProvider.OnChangeStatus += OnChangeStatus;
            }
        }

        private void OnChangeStatus()
        {
            //Debug.Log($"[ErrorServiceProviderFirebase] OnChangeStatus: {_fireBaseProvider.Status}");
            if (_fireBaseProvider.Status == DependencyStatus.Available)
            {
                InitCrashlytics();
            }
        }

        private void InitCrashlytics()
        {
            //Debug.Log("[ErrorServiceProviderFirebase] InitCrashlytics");
            
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
#endif