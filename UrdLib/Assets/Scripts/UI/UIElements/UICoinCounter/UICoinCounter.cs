using Urd;
using Urd.Events;
using Urd.Services;
using TMPro;
using UnityEngine;

namespace Urd
{
    public class UICoinCounter : MonoBehaviour, IEventBusObservable<OnCoinsChangedEvent>
    {
        [SerializeReference, SubclassSelector] 
        private UICoinCounterProvider _provider;
        
        [SerializeField] private TextMeshProUGUI _coinsText;

        void Start()
        {
            Init();
        }

        protected virtual void Init()
        {
            _provider?.Init(this);

            StaticServiceLocator.Get<IEventBusService>().Subscribe(this);
            UpdateData();
        }

        private void OnDestroy()
        {
            StaticServiceLocator.Get<IEventBusService>()?.Unsubscribe(this);
        }

        public void OnNewEvent(OnCoinsChangedEvent newEvent)
        {
            UpdateData();
        }

        public void UpdateData()
        {
            _coinsText.text = GetCoinsString();
        }

        private string GetCoinsString() => _provider == null
            ? "Need Provider"
            :_provider.GetCoinsString();
    }
}
