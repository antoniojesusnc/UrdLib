using Urd.Events;

namespace Urd.Services
{
    public class GamePlayServiceModule : IGamePlayServiceModule,
        IEventBusObservable<OnDummyEvent>
    {
        public virtual void Init()
        {
            StaticServiceLocator.Get<IEventBusService>().Subscribe(this);
        }
        public virtual void BeginGame() { }

        public virtual void Dispose()
        {
            StaticServiceLocator.Get<IEventBusService>()?.Unsubscribe(this);
        }

        public void OnNewEvent(OnDummyEvent newEvent) { }
    }
}