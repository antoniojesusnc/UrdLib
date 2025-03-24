using System.Collections.Generic;
using Urd.Config;
using Urd.Services;

namespace Urd.Gameplay
{
    using System;
    using Urd.Services;

    public interface IGamePlayService : IBaseService
    {
       public bool IsLoading { get; }
        public event Action OnFinishLoad;

        PlayerModel PlayerModel { get; }
        
        public T GetModule<T>() where T : class, IGamePlayServiceModule;

        void LoadData();
    }
}