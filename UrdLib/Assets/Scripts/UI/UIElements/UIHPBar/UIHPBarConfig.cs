using MyBox;
using Urd.Animation;
using Urd.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Urd.UI
{
    [CreateAssetMenu(menuName = "Urd/UI/UIHPBarConfig", fileName = "UIHPBarConfig", order = 1)]
    public class UIHPBarConfig : ScriptableObject
    {
        [field: SerializeField, PreviewSprite]
        public  Sprite BarAsset { get; private set; }
        [field: SerializeField]
        public  Color TextColor { get; private set; }

        [field: Header("Animations")] 
        [field: SerializeField]
        public TweenAnimationHPBarDecrease AnimationWhenHpDown { get; private set; }

        [field:Header("Effect When Low HP")] 
        [field: SerializeField]
        public bool HideWhenFull { get; private set; }
        [field:SerializeField]
        public bool ApplyEffectWhenLowHP { get; private set; }

        [field:SerializeField, ConditionalField("<ApplyEffectWhenLowHP>k__BackingField"), Range(0f, 1f)]
        public float FactorToApplyEffect { get; private set; }

        [field:SerializeField, ConditionalField("<ApplyEffectWhenLowHP>k__BackingField"), PreviewSprite]
        public Sprite BarImageWhenLowHP { get; private set; }

        [field:SerializeField, ConditionalField("<ApplyEffectWhenLowHP>k__BackingField")]
        public Color TextColorWhenLowHP { get; private set; }
    }
}