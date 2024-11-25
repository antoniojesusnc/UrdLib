using System;
using UnityEngine;

namespace Urd.Services
{
    public abstract class PoolServiceProvider : IPoolServiceProvider
    {
        public abstract Type Type { get; }
        
        [field: SerializeField]
        public int Amount { get; private set; }
        public abstract IPoolable OnCreateItem();
    }
}