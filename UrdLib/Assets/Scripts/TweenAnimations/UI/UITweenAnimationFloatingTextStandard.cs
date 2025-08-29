using DG.Tweening;
using MyBox;
using TMPro;
using UnityEngine;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "UITweenAnimationFloatingTextStandard", menuName = "Urd/Animations/UI/UITweenAnimationFloatingTextStandard", order = 1)]
    public class UITweenAnimationFloatingTextStandard : UITweenAnimationFloatingText
    {
        [Header("Text")]
        [SerializeField] 
        public bool _setColor;
        [SerializeField, ConditionalField("_setColor")] 
        public Color _color;
        [SerializeField] 
        public bool _setSize;
        [SerializeField, ConditionalField("_setSize")] 
        public float _size;
        
        [Header("Movement")]
        [SerializeField] 
        public Vector2 _initialPositionOffset;
        [SerializeField] 
        public Vector2 _distanceMove;
        
        public override Tween DoAnimation(RectTransform uiElement, TextMeshProUGUI text)
        {
            var sequence = DOTween.Sequence();

            if (_setColor)
            {
                text.color = _color;
            }

            if (_setSize)
            {
                text.fontSize = _size;
            }
            
            uiElement.anchoredPosition += _initialPositionOffset;
            Tween anchorPositionAnimation = uiElement
                .DOAnchorPos(uiElement.anchoredPosition + _distanceMove, Duration)
                .SetEase(Ease);
            
            sequence.Append(anchorPositionAnimation);
            sequence.SetLink(uiElement.gameObject);
            return sequence;
        }
    }
}
