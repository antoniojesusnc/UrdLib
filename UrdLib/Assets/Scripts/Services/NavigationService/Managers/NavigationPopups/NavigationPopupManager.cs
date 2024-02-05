using System;
using System.Collections.Generic;
using UnityEngine;
using Urd.Error;
using Urd.Utils;

namespace Urd.Navigation
{
    [Serializable]
    public class NavigationPopupManager : NavigationManager<UIPopupModel>
    {
        [SerializeField] 
        private UIPopupConfig _popupConfig;

        private List<UIPopupView> _popupViews;

        private Transform PopupParent
        {
            get
            {
                if (_popupParent == null)
                {
                    _popupParent = GameObject.FindGameObjectWithTag(CanvasTags.PopupCanvas.ToString()).transform;
                }

                return _popupParent;
            }
        }
        private Transform _popupParent;

        public override void Init()
        {
            base.Init();
            _popupViews = new List<UIPopupView>();
        }

        public override void Open(INavigableModel navigableModel, Action<ErrorModel> onOpenNavigable)
        {
            if (!_popupConfig.TryGetPopupView(navigableModel.Type, out var popupView))
            {
                var error = new ErrorModel(
                    $"[NavigationPopupManager] Error when try to get the popup, scene type {navigableModel.Type}",
                    ErrorCode.Error_404_Not_Found);
                Debug.LogWarning(error.ToString());

                onOpenNavigable?.Invoke(error);
            }

            var newView = CreateView(navigableModel as UIPopupModel, popupView);
            _popupViews.Add(newView);
            newView.Open(() => OnOpenView(onOpenNavigable));
        }

        private void OnOpenView(Action<ErrorModel> onOpenNavigable)
        {
            onOpenNavigable?.Invoke(new ErrorModel());
        }


        private UIPopupView CreateView(UIPopupModel popupModel, UIPopupView popupView)
        {
            var newPopupView = GameObject.Instantiate(popupView, PopupParent);
            newPopupView.Init(popupModel);
            return newPopupView;
        }

        public override void Close(INavigableModel navigableModel, Action<ErrorModel> onCloseNavigable)
        {
            var popupView = _popupViews.Find(popupView => popupView.Model.Type.Equals(navigableModel.Type));
            if (popupView != null)
            {
                popupView.Close(() => OnCloseView(popupView, onCloseNavigable));
            }
        }

        public override bool IsOpen(INavigableModel navigableModel)
        {
            var popupModel = navigableModel as UIPopupModel;
            return _popupViews.Exists(popupView => popupView.Model.Type == popupModel.Type);
        }

        private void OnCloseView(UIPopupView uiPopupView, Action<ErrorModel> onOpenNavigable)
        {
            GameObject.Destroy(uiPopupView.gameObject);
            _popupViews.Remove(uiPopupView);
            uiPopupView.Model.PopupClosed();
            onOpenNavigable?.Invoke(new ErrorModel());
        }

        public override void CloseAll()
        {
            for (int i = 0; i < _popupViews.Count; i++)
            {
                Close(_popupViews[i].Model, null);
            }
        }
    }
}