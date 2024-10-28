using System;
using UnityEngine;
using Urd.Navigation;

namespace Urd.Navigation
{
    public class UIBoomerangModel : IDisposable, INavigableModel
    {
        private static int INCREMENTAL_ID = 0; 
        public int Id { get; private set; }
        public event Action OnBoomerangClosed;
        public Enum Type { get; private set; }
        public Transform Parent { get; private set; }
        public Vector3 Position { get; private set; } = Vector3.zero;

        public UIBoomerangModel(Enum boomerangType)
        {
            Id = INCREMENTAL_ID++;
            Type = boomerangType;
        }

        public void BoomerangClosed()
        {
            OnBoomerangClosed?.Invoke();
            OnBoomerangClosed = null;
        }
        
        public virtual void Dispose()
        {
            OnBoomerangClosed = null;
        }

        public void SetParent(Transform parent)
        {
            Parent = parent;
        }
        
        public void SetPosition(Vector3 position)
        {
            Position = position;
        }
    }
}