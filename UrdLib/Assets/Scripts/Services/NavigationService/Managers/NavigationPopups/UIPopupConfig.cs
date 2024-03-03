using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace Urd.Navigation
{
    [CreateAssetMenu(fileName = "UIPopupConfig", menuName = "Urd/Services/UIPopupConfig", order = 1)]
    public class UIPopupConfig : ScriptableObject
    {
        [field: SerializeField, DisplayInspector()]
        public List<UIPopupView> PopupViews { get; private set; } = new List<UIPopupView>();

        public bool TryGetPopupView(Enum popupType, out UIPopupView popupView)
        {
            popupView = PopupViews.Find(dialog => dialog.PopupType.Equals(popupType));
            return popupView != null;
        }
    }
}