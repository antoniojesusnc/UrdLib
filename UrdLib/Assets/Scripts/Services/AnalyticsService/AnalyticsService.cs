using System.Collections.Generic;
using UnityEngine;

namespace Urd.Services
{
    [System.Serializable]
    public class AnalyticsService : BaseService, IAnalyticsService
    {
        public override int LoadPriority => 10;
        
        [SerializeReference, SubclassSelector]
        private List<IAnalyticsServiceProvider> _analyticsServiceProviders = new List<IAnalyticsServiceProvider>();
        
        public override void Init()
        {
            base.Init();
            _analyticsServiceProviders.ForEach(provider => provider.Init());
        }

        public void LogEvent(string eventKey, string eventValue)
        {
            _analyticsServiceProviders.ForEach(provider => provider.LogEvent(eventKey, eventValue));
        }
    }
}