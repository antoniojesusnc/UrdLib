using System;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using UnityEngine.Serialization;

namespace Urd.Navigation
{
    [CreateAssetMenu(fileName = "UIBoomerangConfig", menuName = "Urd/Services/UIBoomerangConfig", order = 1)]
    public class UIBoomerangConfig : ScriptableObject
    {
        [field: SerializeField, DisplayInspector()]
        public List<UIBoomerangView> Boomerangs { get; private set; } = new List<UIBoomerangView>();

        public bool TryGetBoomerangView(Enum boomerangType, out UIBoomerangView boomerangView)
        {
            boomerangView = Boomerangs.Find(boomerang => boomerang.Type.Equals(boomerangType));
            return boomerangView != null;
        }
    }
}