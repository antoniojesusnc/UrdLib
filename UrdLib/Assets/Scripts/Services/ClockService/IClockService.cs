using System;
using UnityEngine;
using Urd.Timer;

namespace Urd.Services
{
    public interface IClockService : IBaseService
    {
        bool IsInPause { get; }
        float DeltaTime { get; }
        DateTime Now { get; }

        void SubscribeToUpdate(Action<float> listener, bool pausable = true);
        void UnSubscribeToUpdate(Action<float> listener);
        void SubscribeToUpdatePerSecond(Action<float> listener, bool pausable = true);
        void UnSubscribeToUpdatePerSecond(Action<float> listener);
        void SubscribeToFixedUpdate(Action<float> listener, bool pausable = true);
        void UnSubscribeToFixedUpdate(Action<float> listener);

        void SetPause(bool gamePaused);

        TimerModel AddDelayCall(float duration, Action finishCallback, bool pausable = true);

        /// <summary>
        /// Method for Test propose, do not use!!
        /// </summary>
        void __TestUpdate(float deltaTime);
    }
}