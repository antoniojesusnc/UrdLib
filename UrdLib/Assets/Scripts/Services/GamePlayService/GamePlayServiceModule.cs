using Urd.Events;

namespace Urd.Services
{
    public class GamePlayServiceModule : IGamePlayServiceModule,
        IEventBusObservable<OnDummyEvent>
    {
        protected IEventBusService _eventBusService;

        public virtual void Init()
        {
            _eventBusService = StaticServiceLocator.Get<IEventBusService>();
            _eventBusService.Subscribe(this);
        }
        public virtual void BeginGame() { }

        public virtual void Dispose()
        {
            _eventBusService?.Unsubscribe(this);
        }

        public void OnNewEvent(OnDummyEvent newEvent) { }
    }
}