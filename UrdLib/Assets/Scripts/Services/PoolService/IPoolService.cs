using System;

namespace Urd.Services
{
    public interface IPoolService : IBaseService
    {
        public void Init<T>(Func<T> onCreate, int amount) where T : class, IPoolable;
        public T Get<T>() where T : class, IPoolable;
        public bool TryGet<T>(out T item) where T : class, IPoolable;
        public void Release<T>(T item) where T : class, IPoolable;
    }
}
