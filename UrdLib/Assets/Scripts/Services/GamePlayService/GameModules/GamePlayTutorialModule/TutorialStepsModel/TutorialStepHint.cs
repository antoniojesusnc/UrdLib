using System;
using Urd.Events;
using Urd.Services;
using UnityEngine;

namespace Urd.Tutorial
{
    [Serializable]
    public class TutorialStepHint : TutorialStep,
        IEventBusObservable<OnBeginGameEvent>
    {
        [field: SerializeField]
        public int PieceSlot { get; private set; }
        
        [field: SerializeField]
        public Vector2Int HintPosition { get; private set; }
        
        [field: SerializeField]
        public float TimeOutToShowHintAgain { get; private set; }
        
        private float _timeout;

        public override void Init()
        {
            base.Init();
            
            StaticServiceLocator.Get<IClockService>().UnSubscribeToUpdate(CustomUpdate);
            StaticServiceLocator.Get<IClockService>().SubscribeToUpdate(CustomUpdate);

            _timeout = -1;
        }

        private void CustomUpdate(float deltaTime)
        {
            if (_timeout < 0)
            {
                return;
            }
            
            _timeout -= deltaTime;
            if (_timeout < 0)
            {
            }
        }

        public override void Finish()
        {
            base.Finish();
            _timeout = -1;
            StaticServiceLocator.Get<IEventBusService>().Unsubscribe(this);
            StaticServiceLocator.Get<IClockService>().UnSubscribeToUpdate(CustomUpdate);
        }

        public override void Begin()
        {
            base.Begin();
            StaticServiceLocator.Get<IEventBusService>().Subscribe(this);
        }

        public void OnNewEvent(OnBeginGameEvent newEvent)
        {
            _timeout = TimeOutToShowHintAgain;
        }

    }
}