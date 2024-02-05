using System;

namespace Urd.Error
{
    public interface IErrorNetworkModel : IDisposable
    {
        string Details { get; }
    }
}