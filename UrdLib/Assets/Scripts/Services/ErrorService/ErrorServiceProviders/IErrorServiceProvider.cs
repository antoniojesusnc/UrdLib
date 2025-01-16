using System;

namespace Urd.Services
{
    public interface IErrorServiceProvider : IDisposable
    {
        void Init();
        void LogError(string message);
    }
}