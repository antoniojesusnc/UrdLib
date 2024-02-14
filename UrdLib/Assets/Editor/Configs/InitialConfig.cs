using System.IO;
using UnityEditor;
using UnityEngine;
using Urd.Navigation;
using Urd.Services;
using Urd.Utils;

namespace Urd.Editor
{
    public class InitialConfig : MonoBehaviour
    {
        private const string CONFIG_FOLDER = "Configurations";
        private const string SERVICES_FOLDER = "Services";
        private const string FILE_NAME_FORMAT = "ServiceLocatorConfig{0}.asset";
        
        private const string CONFIG_FILE_POPUP = "UIPopupConfig{0}.asset";
        private const string CONFIG_FILE_BOOMERANG = "UIBoomerangConfig{0}.asset";
        private const string CONFIG_FILE_DOTWEEEN_ANIMATION = "DotweenAnimationsConfig{0}.asset";
        private const string CONFIG_FILE_NOTIFICATION = "NotificatinsConfig{0}.asset";

        private static string Folder => $"{Application.dataPath}/{CONFIG_FOLDER}";
        private static string ServicesFolder => $"{Application.dataPath}/{CONFIG_FOLDER}/{SERVICES_FOLDER}";
        private static string RelativeFolder => $"Assets/{CONFIG_FOLDER}";
        private static string RelativeServiceFolder => $"Assets/{CONFIG_FOLDER}/{SERVICES_FOLDER}";

        private static ServiceLocatorConfig _serviceLocatorConfig;
        
        [MenuItem("Urd/Initial Configuration")]
        public static void CreateInitialConfig()
        {
            CreateConfigFolder();
            CreateServiceLocatorConfig();
            AddConfigFiles();
        }
        
        private static void CreateConfigFolder()
        {
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }
            if (!Directory.Exists(ServicesFolder))
            {
                Directory.CreateDirectory(ServicesFolder);
            }
        }
        
        private static void CreateServiceLocatorConfig()
        {
            _serviceLocatorConfig = CreateConfig<ServiceLocatorConfig>(FILE_NAME_FORMAT, false);
            _serviceLocatorConfig.FillWithAllServices();
            AssetDatabase.SaveAssets();
            Debug.Log($"New Service locator config with path \" {_serviceLocatorConfig.name} \" created");
        }
        
        private static T CreateConfig<T>(string fileNameFormat, bool isAServiceConfig = true) where T : ScriptableObject
        {
            var config = ScriptableObject.CreateInstance<T>();

            var filePath = $"{(isAServiceConfig?RelativeServiceFolder:RelativeFolder)}/{string.Format(fileNameFormat, "")}";
            var file = AssetDatabase.LoadAssetAtPath<ServiceLocatorConfig>(filePath);
            if (file != null)
            {
                filePath = GetNextFilePath<T>(fileNameFormat, isAServiceConfig);
                if (filePath == null)
                {
                    Debug.LogError("You already have more than 100 fileNameFormat in the folder");
                }
            }

            AssetDatabase.CreateAsset(config, filePath); 
            AssetDatabase.SaveAssets();

            return config;
        }

        private static string GetNextFilePath<T>(string fileNameFormat, bool isAServiceConfig) where T : ScriptableObject
        {
            string filePath = "";
            for (int i = 1; i < 100; i++)
            {
                filePath = $"{(isAServiceConfig?RelativeServiceFolder:RelativeFolder)}/{string.Format(fileNameFormat, i.ToString("00"))}";
                var file = AssetDatabase.LoadAssetAtPath<T>(filePath);
                if (file == null)
                {
                    return filePath;
                }
            }

            return null;
        }
        private static void AddConfigFiles()
        {
            AddPopupConfig();
            FillPopupConfig();
            AddBoomerangConfig();
            AddDotweenAnimationConfig();
            AddNotificationConfig();
            
            
            AssetDatabase.SaveAssets();
        }


        private static void AddPopupConfig()
        {
            var uiPopupConfig = CreateConfig<UIPopupConfig>(CONFIG_FILE_POPUP);
            var navigationService = _serviceLocatorConfig.ListOfServices.Find(
                service => service.GetMainInterface().IsAssignableFrom(typeof(INavigationService))) as INavigationService;
            var navigationPopupManager =
                navigationService.NavigationManagers.Find(
                    navigationManager => navigationManager.GetType().IsAssignableFrom(typeof(NavigationPopupManager))) as NavigationPopupManager;
            navigationPopupManager.SetConfig(uiPopupConfig);
        }
        private static void FillPopupConfig()
        {
            var popupViews = AssemblyHelper.GetClassTypesThatImplement<UIPopupView>();
            Debug.Log(popupViews);
        }
        
        private static void AddBoomerangConfig()
        {
            var uiBoomerangConfig = CreateConfig<UIBoomerangConfig>(CONFIG_FILE_BOOMERANG);
            var navigationService = _serviceLocatorConfig.ListOfServices.Find(
                service => service.GetMainInterface().IsAssignableFrom(typeof(INavigationService))) as INavigationService;
            var navigationBoomerangManager =
                navigationService.NavigationManagers.Find(
                    navigationManager => navigationManager.GetType().IsAssignableFrom(typeof(NavigationBoomerangManager))) as NavigationBoomerangManager;
            navigationBoomerangManager.SetConfig(uiBoomerangConfig);
        }
        
        private static void AddDotweenAnimationConfig()
        {
            var dotweenAnimationConfig = CreateConfig<DotweenAnimationConfig>(CONFIG_FILE_DOTWEEEN_ANIMATION);
            var dotweenAnimationService = _serviceLocatorConfig.ListOfServices.Find(
                service => service.GetMainInterface().IsAssignableFrom(typeof(IDotweenAnimationService))) as IDotweenAnimationService;
            dotweenAnimationService.SetConfig(dotweenAnimationConfig);
        }
        
        private static void AddNotificationConfig()
        {
            var notificationServiceConfig = CreateConfig<NotificationServiceConfig>(CONFIG_FILE_NOTIFICATION);
            var notificationService = _serviceLocatorConfig.ListOfServices.Find(
                service => service.GetMainInterface().IsAssignableFrom(typeof(INotificationService))) as INotificationService;
            notificationService.SetConfig(notificationServiceConfig);
        }
    }
}
