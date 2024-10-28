using System;

namespace Urd.Navigation
{
    public class UIPopupModel : IDisposable, INavigableModel
    {
        private static int INCREMENTAL_ID = 0; 
        public int Id { get; private set; }
        public event Action OnPopupClosed;
        
        public Enum Type { get; private set; }
        public UIPopupModel(Enum popupType)
        {
            Id = INCREMENTAL_ID++;
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