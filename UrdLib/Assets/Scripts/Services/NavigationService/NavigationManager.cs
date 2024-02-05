using System;
using Urd.Error;

namespace Urd.Navigation
{
    [Serializable]
    public abstract class NavigationManager<TModelType>: INavigationManager
    {
        public virtual void Init(){ }
        public Type ModelType => typeof(TModelType);
        public abstract void Open(INavigableModel navigableModel, Action<ErrorModel> onOpenNavigable);
        public abstract void Close(INavigableModel navigableModel, Action<ErrorModel> onCloseNavigable);
        public abstract bool IsOpen(INavigableModel navigableModel);
        public virtual void CloseAll() { }
    }
}