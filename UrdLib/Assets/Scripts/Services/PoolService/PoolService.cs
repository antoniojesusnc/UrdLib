using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Urd.Services
{
    [Serializable]
    public class PoolService : BaseService, IPoolService
    {
        [field: SerializeReference, SubclassSelector]
        private List<IPoolServiceProvider> _poolServiceProviders = new List<IPoolServiceProvider>();
        
        private Dictionary<Type, ObjectPool<IPoolable>> _objectsPool = new Dictionary<Type, ObjectPool<IPoolable>>();
        
        public override int LoadPriority => 50;

        public override void Init()
        {
            base.Init();
            _objectsPool.Clear();
            InitPoolProviders();
        }

        private void InitPoolProviders()
        {
            for (int i = 0; i < _poolServiceProviders.Count; i++)
            {
                var poolInfo = _poolServiceProviders[i];
                var objectPool = new ObjectPool<IPoolable>(poolInfo.OnCreateItem, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, true, poolInfo.Amount, poolInfo.MaxAmount);
                _objectsPool.Add(poolInfo.Type, objectPool);
            }
        }
        
        public void Init<T>(Func<T> onCreate, int amount) where T : class, IPoolable
        {
            var objectPool = new ObjectPool<IPoolable>(onCreate, OnTakeFromPool, OnReturnToPool, OnDestroyPoolObject, true, amount);
            _objectsPool.Add(typeof(T), objectPool);
        }
        
        private IPoolable OnCreatePoolItem()
        {
            return default;
        }
        
        private void OnTakeFromPool(IPoolable item)
        {
            item.OnGet();
        }
        
        private void OnReturnToPool(IPoolable item)
        {
            item.OnRelease();
        }
        
        private void OnDestroyPoolObject(IPoolable item)
        {
            item.Dispose();
        }

        public T Get<T>() where T : class, IPoolable
        {
            if (TryGet<T>(out var item))
            {
                return item;
            }

            return null;
        }

        public bool TryGet<T>(out T item) where T : class, IPoolable
        {
            item = null;
            if (_objectsPool.TryGetValue(typeof(T), out var pool) && IsNotMax<T>(pool))
            {
                item = pool.Get() as T;
                return true;
            }

            return false;
        }

        private bool IsNotMax<T>(ObjectPool<IPoolable> pool) where T : class, IPoolable
        {
            var provider = _poolServiceProviders.Find(providerData => providerData.Type == typeof(T));
            if (provider == null)
            {
                return true;
            }
            
            return pool.CountInactive > 0 || pool.CountActive <= provider.Amount;
        }

        public void Release<T>(T item) where T : class, IPoolable
        {
            if (item != null && _objectsPool.TryGetValue(typeof(T), out var pool))
            {
                pool.Release(item);
            }
        }
    }
}
