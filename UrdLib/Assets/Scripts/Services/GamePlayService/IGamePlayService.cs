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

        public T GetPlayerModel<T>() where T : class, IPlayerModel;
        public T GetModule<T>() where T : class, IGamePlayModule;

        void LoadData();
    }
}