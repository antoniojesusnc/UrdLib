using System.Collections.Generic;
using UnityEngine;

namespace Urd.Services
{
    [System.Serializable]
    public class ErrorService : BaseService, IErrorService
    {
        public override int LoadPriority => 10;
        
        [field: SerializeField]
        public bool IsBannerEnabled { get; private set; }

        [SerializeReference, SubclassSelector]
        private List<IErrorServiceProvider> _errorServiceProvider = new List<IErrorServiceProvider>();
        
        public override void Init()
        {
            base.Init();
            _errorServiceProvider.ForEach(provider => provider.Init());
        }
    }
}