using System;
using System.Collections;

namespace Urd
{
    public interface IGamePlayModule : IDisposable
    {
        void Init();
        IEnumerator BeginGameCoroutine();
        void BeginBattle();
        void GameOver(bool isWon);
    }
}
