using Urd.Character;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Urd
{
    public class UICharacterStat : MonoBehaviour
    {
        [SerializeField] 
        private Image _symbol;
        
        [SerializeField] 
        private TextMeshProUGUI _statNameText;

        [SerializeField] 
        private TextMeshProUGUI _stateValueText;

        private AttributeModel _attributeModel;
        
        public void SetAttribute(AttributeModel attributeModel)
        {
            _attributeModel = attributeModel;

            OnEnable();
            UpdateView();
        }

        private void OnEnable()
        {
            if (_attributeModel != null)
            {
                _attributeModel.OnChangedCurrent -= OnChangedCurrent;
                _attributeModel.OnChangedCurrent += OnChangedCurrent;
            }
        }
        
        private void OnDisable()
        {
            if (_attributeModel != null)
            {
                _attributeModel.OnChangedCurrent -= OnChangedCurrent;
            }
        }

        private void OnChangedCurrent(double value)
        {
            UpdateView();
        }

        protected virtual void UpdateView()
        {
            _stateValueText.text = _attributeModel.Current.ToString();
        }
    }
}