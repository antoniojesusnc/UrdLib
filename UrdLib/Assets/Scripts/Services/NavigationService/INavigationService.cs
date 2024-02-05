using System;
using System.Collections.Generic;
using Urd.Error;
using Urd.Navigation;

namespace Urd.Services
{
    public interface INavigationService : IBaseService
    {
        List<INavigationManager> NavigationManagers { get; }
        event Action<INavigableModel> OnNavigableOpened;
        event Action OnCloseAll;
        void Open(INavigableModel navigableModel, Action<ErrorModel> onOpenNavigableCallback = null);
        void Close(INavigableModel navigableModel, Action<ErrorModel> callback = null);
        bool IsOpen(INavigableModel navigableModel);
        void CloseAll<T>() where T : INavigableModel;
    }
}
