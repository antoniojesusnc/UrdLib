using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Urd.UI
{
    public class UIStyleApplicator : MonoBehaviour
    {
        [SerializeField]
        private UIStyleConfig _config;
        
        [field: SerializeField]
        public bool HasToApplyMainBackgroundColor { get; private set; }
        [field: SerializeField]
        public bool HasToApApplyMainFont { get; private set; }
        [field: SerializeField]
        public bool HasToApApplySecondaryFont { get; private set; }
        
        private void OnEnable()
        {
            if (HasToApplyMainBackgroundColor)
            {
                ApplyMainBackgroundColor();
            }
            
            if (HasToApApplyMainFont)
            {
                ApplyMainFont();
            }
            
            if (HasToApApplySecondaryFont)
            {
                ApplySecondaryFont();
            }
        }

        private void ApplyMainBackgroundColor()
        {
            var image = GetComponent<Image>();
            if (image != null && image.color != _config.MainBackgroundColor)
            {
                image.color = _config.MainBackgroundColor;
            }
        }

        private void ApplyMainFont()
        {
            var text = GetComponent<TextMeshProUGUI>();
            if (text != null && text.font != _config.MainFont)
            {
                text.font = _config.SecondaryFont;
            }
        }

        private void ApplySecondaryFont()
        {
            var text = GetComponent<TextMeshProUGUI>();
            if (text != null && text.font != _config.SecondaryFont)
            {
                text.font = _config.SecondaryFont;
            }
        }
    }
}