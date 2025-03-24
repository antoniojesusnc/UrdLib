namespace Urd.Services
{
    using System;

    public interface IGamePlayServiceModule : IDisposable
    {
        void Init();
        void BeginGame();
    }
}
