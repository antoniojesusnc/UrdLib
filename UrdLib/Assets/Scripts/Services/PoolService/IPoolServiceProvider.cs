using System;

namespace Urd.Services
{
    public interface IPoolServiceProvider
    {
        Type Type { get; }
        int Amount { get; }
        int MaxAmount { get; }
        IPoolable OnCreateItem();
    }
}