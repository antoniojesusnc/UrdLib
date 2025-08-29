using System;
using MyBox;
using Urd.Utils;
using UnityEngine;

namespace Urd.Tutorial
{
    [Serializable]
    public class TutorialStepHighlightAreaMessage : TutorialStep
    {
        [field: Header("Message")]
        [field: SerializeField]
        public bool HasMessage { get; private set; }
        [field: SerializeField, ConditionalField("<HasMessage>k__BackingField")]
        public string Message { get; private set; }
        [field: SerializeField, ConditionalField("<HasMessage>k__BackingField")]
        public int MessageFontSize { get; private set; }

        [field: SerializeField, ConditionalField("<HasMessage>k__BackingField")]
        public Vector3 MessageOffset { get; private set; }
        
        [field: Header("Image")]
        [field: SerializeField]
        public bool HasImage { get; private set; }
        [field: SerializeField, PreviewSprite, ConditionalField("<HasImage>k__BackingField")]
        public Sprite Image { get; private set; }

        [field: SerializeField, ConditionalField("<HasImage>k__BackingField")]
        public Vector3 ImageSize { get; private set; }
        [field: SerializeField, ConditionalField("<HasImage>k__BackingField")]
        public Vector3 ImageOffset { get; private set; }
        [field: SerializeField, ConditionalField("<HasImage>k__BackingField")]
        public Vector3 ImageRotation { get; private set; }
        [field: SerializeField, ConditionalField("<HasImage>k__BackingField")]
        public bool FlipX { get; private set; }
        [field: SerializeField, ConditionalField("<HasImage>k__BackingField")]
        public bool FlipY { get; private set; }

        [field: Header("Behavior")]
        [field: SerializeField]
        public bool DetectClicks { get; private set; } = true;
        [field: SerializeField]
        public bool HighlightArea { get; private set; } = true;
        [field: SerializeField] 
        public string UiObjectToFocusToSearch { get; private set; }
        [field: SerializeField] 
        public Vector3 UiObjectToFocusToSearchOffset { get; private set; }
        [field: SerializeField] 
        public Vector2 AreaSize { get; private set; }
        
        [field: SerializeField]
        public float DelayToShow { get; private set; }

        public override void Begin()
        {
            base.Begin();
        }

        public override void Finish()
        {
            base.Finish();
        }

        public void OnClickInArea()
        {
            TutorialModule.CompleteTutorialStep(this);
        }
    }
}