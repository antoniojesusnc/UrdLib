using System;

namespace Urd.Services
{
    public abstract class BaseService : IBaseService
    {
        public abstract int LoadPriority { get; }
        public virtual void Init() { }

        protected virtual bool IsLoaded { get; set; } = true;

        protected event Action OnServiceFinishLoad;

        protected void SetAsLoaded()
        {
            IsLoaded = true;
            OnServiceFinishLoad?.Invoke();
        }
        
        public Type GetMainInterface()
        {
            var interfaces = GetType().GetInterfaces();
            Type iBaseServiceType = typeof(IBaseService);
            for (int i = 0; i < interfaces.Length; i++)
            {
                if (iBaseServiceType.IsAssignableFrom(interfaces[i]) && iBaseServiceType != interfaces[i])
                {
                    return interfaces[i];
                }
            }

            return null;
        }

        public virtual void Dispose() { }
    }
}