using System;
using System.Collections.Generic;
using UnityEngine;
using Urd.Error;
using Urd.Utils;

namespace Urd.Navigation
{
    [Serializable]
    public class NavigationBoomerangManager : NavigationManager<UIBoomerangModel>
    {
        [SerializeField] 
        private UIBoomerangConfig _boomerangConfig;

        private List<UIBoomerangView> _boomerangViews;

        private Transform BoomerangParent
        {
            get
            {
                if (_boomerangParent == null)
                {
                    _boomerangParent = GameObject.FindGameObjectWithTag(CanvasTags.BoomerangCanvas.ToString()).transform;
                }

                return _boomerangParent;
            }
        }
        private Transform _boomerangParent;

        public override void SetConfig(ScriptableObject config)
        {
            _boomerangConfig = config as UIBoomerangConfig;
        } 
        
        public override void Init()
        {
            base.Init();
            _boomerangViews = new List<UIBoomerangView>();
        }

        public override void Open(INavigableModel navigableModel, Action<ErrorModel> onOpenNavigable)
        {
            if (!_boomerangConfig.TryGetBoomerangView(navigableModel.Type, out var boomerangView))
            {
                var error = new ErrorModel(
                    $"[NavigationBoomerangManager] Error when try to get the boomerang, scene type {navigableModel.Type}",
                    ErrorCode.Error_404_Not_Found);
                Debug.LogWarning(error.ToString());

                onOpenNavigable?.Invoke(error);
            }

            var newView = CreateView(navigableModel as UIBoomerangModel, boomerangView);
            _boomerangViews.Add(newView);
            newView.Open(() => OnOpenView(onOpenNavigable));
        }

        private void OnOpenView(Action<ErrorModel> onOpenNavigable)
        {
            onOpenNavigable?.Invoke(new ErrorModel());
        }


        private UIBoomerangView CreateView(UIBoomerangModel boomerangModel, UIBoomerangView boomerangView)
        {
            var parent = boomerangModel.Parent != null? boomerangModel.Parent: BoomerangParent;
            var newBoomerangView = GameObject.Instantiate(boomerangView, parent);
            newBoomerangView.Init(boomerangModel);
            return newBoomerangView;
        }

        public override void Close(INavigableModel navigableModel, Action<ErrorModel> onCloseNavigable)
        {
            var boomerangView = _boomerangViews.Find(boomerangView => boomerangView.Model.Type.Equals(navigableModel.Type));
            if (boomerangView != null)
            {
                boomerangView.Close(() => OnCloseView(boomerangView, onCloseNavigable));
            }
        }

        public override bool IsOpen(INavigableModel navigableModel)
        {
            var boomerangModel = navigableModel as UIBoomerangModel;
            return _boomerangViews.Exists(boomerangView => boomerangView.Model.Type == boomerangModel.Type);
        }

        private void OnCloseView(UIBoomerangView uiBoomerangView, Action<ErrorModel> onOpenNavigable)
        {
            GameObject.Destroy(uiBoomerangView.gameObject);
            _boomerangViews.Remove(uiBoomerangView);
            uiBoomerangView.Model.BoomerangClosed();
            onOpenNavigable?.Invoke(new ErrorModel());
        }

        public override void CloseAll()
        {
            for (int i = 0; i < _boomerangViews.Count; i++)
            {
                Close(_boomerangViews[i].Model, null);
            }
        }
    }
}