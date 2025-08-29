using System;
using Urd.Audio;
using Urd.Services;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Urd.UI
{
    public class UIToggle : MonoBehaviour
    {
        private const string ON_TEXT = "ON"; 
        private const string OFF_TEXT = "OFF";
        
        [Header("Preview Value")]
        [field: SerializeField]
        private bool _preview;
            
        [Header("Configuration From File")]
        [SerializeField] private UIToggleConfig _config;

        [Header("Config If don't exist Config File")] 
        [Header("On")] [SerializeField]
        private Color _onBackgroundColor;
        [field: SerializeField] 
        private Color _onTextColor;
        
        [field: Header("Off")] 
        [field: SerializeField]
        private Color _offBackgroundColor;
        [field: SerializeField] 
        private Color _offTextColor;

        [Header("Component")] 
        [SerializeField]
        private Image _background;
        [SerializeField]
        private TextMeshProUGUI _text;
        [SerializeField]
        private RectTransform _handleRectTransform;
        [SerializeField]
        private RectTransform _onPosition;
        [SerializeField]
        private RectTransform _offPosition;
        
        public event Action<bool> OnChanged;

        public bool IsOn { get; private set; }

        void Start()
        {
        }

        private void UpdateView()
        {
            _background.color = GetBackgroundColor();
            _text.color = GetTextColor();
            _text.text = IsOn ? ON_TEXT : OFF_TEXT;
            _handleRectTransform.anchoredPosition = IsOn ? _onPosition.anchoredPosition : _offPosition.anchoredPosition;
        }

        private Color GetBackgroundColor()
        {
            if (_config != null)
            {
                return IsOn ? _config.OnBackgroundColor : _config.OffBackgroundColor;
            }

            return IsOn ? _onBackgroundColor : _offBackgroundColor;
        }
        
        private Color GetTextColor()
        {
            if (_config != null)
            {
                return IsOn ? _config.OnTextColor : _config.OffTextColor;
            }

            return IsOn ? _onTextColor : _offTextColor;
        }


        public void OnClick()
        {
            SetStatus(!IsOn);
            StaticServiceLocator.Get<IAudioService>().PlaySound(AudioGenericType.Click);
        }

        public void SetStatus(bool isOn)
        {
            IsOn = isOn;
            UpdateView();
            
            OnChanged?.Invoke(IsOn);
        }
        
        #if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                if (_preview != IsOn)
                {
                    SetStatus(_preview);
                }
            }
        }
        #endif
    }
}