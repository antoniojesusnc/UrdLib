using UnityEngine;
using Urd.Services;

namespace Urd.Events
{
    public class MonoBehaviourEventObserver : MonoBehaviour,
        IEventBusObservable<OnDummyEvent>
    {
        public virtual void Start()
        {
            StaticServiceLocator.Get<IEventBusService>().Subscribe(this);
        }
        
        public virtual void OnDestroy()
        {
            StaticServiceLocator.Get<IEventBusService>()?.Unsubscribe(this);
        }

        public void OnNewEvent(OnDummyEvent newEvent)
        {
        }
    }
}