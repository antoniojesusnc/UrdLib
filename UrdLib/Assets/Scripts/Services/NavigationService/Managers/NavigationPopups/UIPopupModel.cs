using System;

namespace Urd.Navigation
{
    public class UIPopupModel : IDisposable, INavigableModel
    {
        public event Action OnPopupClosed;
        
        public Enum Type { get; private set; }
        public UIPopupModel(Enum popupType)
        {
            Type = popupType;
        }

        public void PopupClosed()
        {
            OnPopupClosed?.Invoke();
            OnPopupClosed = null;
        }
        
        public void Dispose()
        {
            
        }

    }
}