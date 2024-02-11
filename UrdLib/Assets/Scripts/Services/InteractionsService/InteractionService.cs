using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Urd.Inputs;
using Urd.Navigation;

namespace Urd.Services
{

    public class InteractionService : BaseService, IInteractionService
    {
        public override int LoadPriority => 200;

        private List<CameraClickability> _cameraClickability = new List<CameraClickability>();

        public override void Init()
        {
            base.Init();

            StaticServiceLocator.Get<INavigationService>().OnNavigableOpened += OnNavigableOpened;
        }

        private void OnNavigableOpened(INavigableModel navigableModel)
        {
            if (navigableModel.GetType().IsAssignableFrom(typeof(SceneModel)))
            {
                GetAllCameraClickability();
            }
        }

        private void GetAllCameraClickability()
        {
            var clickabilities = GameObject.FindObjectsOfType<CameraClickability>(true);
            if (clickabilities?.Length > 0)
            {
                _cameraClickability = new List<CameraClickability>(clickabilities);
            }
            _cameraClickability.ForEach(clickabilities => clickabilities.SetClickablity(true));
        }
    }
}
