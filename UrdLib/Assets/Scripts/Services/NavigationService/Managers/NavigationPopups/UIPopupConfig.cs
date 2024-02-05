using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace Urd.Navigation
{
    [CreateAssetMenu(fileName = "UIPopupConfig", menuName = "Urd/Services/UIPopupConfig", order = 1)]
    public class UIPopupConfig : ScriptableObject
    {
        [SerializeField, DisplayInspector()]
        private List<UIPopupView> _dialogs;

        public bool TryGetPopupView(Enum popupType, out UIPopupView popupView)
        {
            popupView = _dialogs.Find(dialog => dialog.PopupType.Equals(popupType));
            return popupView != null;
        }
    }
}