using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;

namespace Urd.Navigation
{
    [CreateAssetMenu(fileName = "UIBoomerangConfig", menuName = "Urd/Services/UIBoomerangConfig", order = 1)]
    public class UIBoomerangConfig : ScriptableObject
    {
        [SerializeField, DisplayInspector()]
        private List<UIBoomerangView> _boomerangs;

        public bool TryGetBoomerangView(Enum boomerangType, out UIBoomerangView boomerangView)
        {
            boomerangView = _boomerangs.Find(boomerang => boomerang.BoomerangType.Equals(boomerangType));
            return boomerangView != null;
        }
    }
}