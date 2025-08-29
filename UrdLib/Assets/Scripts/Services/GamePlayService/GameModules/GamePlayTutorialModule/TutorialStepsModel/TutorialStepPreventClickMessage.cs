using System;
using Urd.Services;
using UnityEngine;

namespace Urd.Tutorial
{
    [Serializable]
    public class TutorialStepPreventClickMessage : TutorialStep
    {
        [field: SerializeField]
        public string Message { get; private set; }
    }
}