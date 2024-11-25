using System;

namespace Urd
{
    public interface IPoolable : IDisposable
    {
        void OnGet();
        void OnRelease();
    }
}