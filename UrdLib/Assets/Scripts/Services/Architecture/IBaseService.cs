using System;

namespace Urd.Services
{
    public interface IBaseService 
    {
        public int LoadPriority { get; }
        void Init();
        Type GetMainInterface();
    }
}