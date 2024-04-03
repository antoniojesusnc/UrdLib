using System;
using UnityEngine;
using Urd.Navigation;

namespace Urd.Navigation
{
    public class UIBoomerangModel : IDisposable, INavigableModel
    {
        public event Action OnBoomerangClosed;
        public Enum Type { get; private set; }
        public Transform Parent { get; set; }
        
        public UIBoomerangModel(Enum boomerangType)
        {
            Type = boomerangType;
        }

        public void BoomerangClosed()
        {
            OnBoomerangClosed?.Invoke();
            OnBoomerangClosed = null;
        }
        
        public void Dispose()
        {
            
        }

        public void SetParent(Transform parent)
        {
            Parent = parent;
        }
    }
}