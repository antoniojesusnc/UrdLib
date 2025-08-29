using System;
using MyBox;
using Urd.Character;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Urd.UI
{
    public class UIHPBar : MonoBehaviour
    {
        [SerializeField, DisplayInspector]
        private UIHPBarConfig _config;
        
        [SerializeField] private bool _hideWhenFull;
        [SerializeField] private Image _bar;
        [SerializeField] private TextMeshProUGUI _barText;
        [Header("Effect When Low HP")]
        [SerializeField] 
        private bool _applyEffectWhenLowHP;
        [SerializeField, Range(0f,1f), ConditionalField("_applyEffectWhenLowHP")] 
        private float _factorToApplyEffect;
        [SerializeField, ConditionalField("_applyEffectWhenLowHP")] 
        private Sprite _barImageWhenLowHP;
        [SerializeField, ConditionalField("_applyEffectWhenLowHP")] 
        private Color _textColorWhenLowHP;
        
        private AttributeModel _hpAttribute;

        private Sprite _originalBarAsset;
        private Color _originalTextColor;

        private void Awake()
        {
            _originalBarAsset = _bar.sprite;
            _originalTextColor = _barText.color;
            SetForConfig();
            
            gameObject.SetActive(!_hideWhenFull);
        }

        private void SetForConfig()
        {
            if (_config == null)
            {
                return;
            }

            _originalBarAsset = _config.BarAsset ?? _bar.sprite;
            _originalTextColor = _config.TextColor == Color.clear
                ? _barText.color
                : _config.TextColor;
            
            _hideWhenFull = _config.HideWhenFull;
            _applyEffectWhenLowHP = _config.ApplyEffectWhenLowHP;
            _factorToApplyEffect = _config.FactorToApplyEffect;
            _barImageWhenLowHP = _config.BarImageWhenLowHP ?? _bar.sprite;
            _textColorWhenLowHP = _config.TextColorWhenLowHP == Color.clear
                ? _originalTextColor
                :_config.TextColorWhenLowHP;
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        public void SetModel(AttributeModel hpAttribute)
        {
            _hpAttribute = hpAttribute;
            _hpAttribute.OnChangedCurrent += OnChangeHp;
            
            SetData();
        }

        private void Unsubscribe()
        {
            if (_hpAttribute != null)
            {
                _hpAttribute.OnChangedCurrent -= OnChangeHp;
            }
        }

        private void OnChangeHp(double obj)
        {
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            _config?.AnimationWhenHpDown?.DoAnimation(this);
            SetData();
        }

        private void SetData()
        {
            var factor = _hpAttribute.Factor;
            
            gameObject.SetActive(!_hideWhenFull || factor < 1);
            
            _bar.fillAmount = factor;

            if (!_applyEffectWhenLowHP)
            {
                _barText.text = $"{Math.Round(_hpAttribute.Current)}/{_hpAttribute.MaxWithModifiers()}";
            }
            else
            {
                _bar.sprite = factor <= _factorToApplyEffect ? _barImageWhenLowHP : _originalBarAsset;
                var color = factor <= _factorToApplyEffect ? _textColorWhenLowHP : _originalTextColor;
                _barText.text = $"<color={color.ToHex()}>{Math.Round(_hpAttribute.Current)}</color>/{_hpAttribute.MaxWithModifiers()}";
            }
        }
    }
}
