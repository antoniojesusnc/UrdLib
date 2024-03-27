using System;
using UnityEngine;

namespace Urd.Navigation
{
    public class UIPopupViewT<T> : UIPopupView where T : Enum
    {
        [field: SerializeField] public T PopupType { get; private set; }

        public override Enum Type => PopupType;
    }
}
