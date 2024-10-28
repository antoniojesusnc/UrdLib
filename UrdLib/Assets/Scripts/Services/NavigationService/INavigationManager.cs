using System;
using UnityEngine;
using Urd.Error;

namespace Urd.Navigation
{
    public interface INavigationManager
    {
        Type ModelType { get; }
        void Init();   
        void Open(INavigableModel navigableModel, Action<ErrorModel> onOpenNavigable);
        void Close(INavigableModel navigableModel, Action<ErrorModel> callback);
        bool IsOpen(INavigableModel navigableModel);
        bool IsOpenAny();
        void SetConfig(ScriptableObject config);
        void CloseAll();
    }
}