using System;
using UnityEngine;

namespace Urd.Navigation
{
    public class UIBoomerangViewT<T> : UIBoomerangView where T : Enum
    {
        [field: SerializeField] public T BoomerangType { get; private set; }

        public override Enum Type => BoomerangType;
    }
}
