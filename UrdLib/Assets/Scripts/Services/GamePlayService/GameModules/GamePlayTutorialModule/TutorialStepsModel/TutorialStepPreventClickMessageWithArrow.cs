using System;
using Urd.Events;
using Urd.Services;
using UnityEngine;
using Urd.Gameplay;

namespace Urd.Tutorial
{
    [Serializable]
    public class TutorialStepPreventClickMessageWithArrow : TutorialStep, 
        IEventBusObservable<OnBeginGameEvent>
    {
        private const string FTUE_HP = "FTUE_HP";
        
        [field: SerializeField]
        public string Message { get; private set; }
        
        [field: SerializeField]
        public bool UseArrow { get; private set; }
        [field: SerializeField]
        public Vector3 ArrowPosition { get; private set; }

        public override void Init()
        {
            if(!StaticServiceLocator.Get< IGamePlayService>().GetPlayerModel<IPlayerModel>().TutorialsCompleted.Contains(FTUE_HP))
            //if(!StaticServiceLocator.Get<ISaveLoadService>().Load(FTUE_HP, false)) 
                StaticServiceLocator.Get<IEventBusService>().Subscribe(observer: this);
        }
        
        public override void Finish()
        {
            base.Finish();
            StaticServiceLocator.Get<IGamePlayService>().GetPlayerModel<IPlayerModel>().TutorialsCompleted.Add(FTUE_HP);
            //StaticServiceLocator.Get<ISaveLoadService>().Save(FTUE_HP, true);
            StaticServiceLocator.Get<IEventBusService>().Unsubscribe(observer: this);

        }

        public void OnNewEvent(OnBeginGameEvent newEvent)
        {
        }
    }
}