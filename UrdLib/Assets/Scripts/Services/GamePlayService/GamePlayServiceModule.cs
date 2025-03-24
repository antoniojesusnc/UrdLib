namespace Urd.Services
{
    public class GamePlayServiceModule : IGamePlayServiceModule
    {
        public virtual void Init() { }
        public virtual void BeginGame() { }

        public virtual void Dispose() { }
    }
}