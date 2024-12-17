using System.Collections.Generic;
using UnityEngine;

namespace Urd.Services
{
    [System.Serializable]
    public class ErrorService : BaseService, IErrorService
    {
        public override int LoadPriority => 10;
        
        [SerializeReference, SubclassSelector]
        private List<IErrorServiceProvider> _errorServiceProvider = new List<IErrorServiceProvider>();
        
        public override void Init()
        {
            base.Init();
            _errorServiceProvider.ForEach(provider => provider.Init());
        }

        public void LogError(string message)
        {
            _errorServiceProvider.ForEach(provider => provider.LogError(message));
        }
    }
}