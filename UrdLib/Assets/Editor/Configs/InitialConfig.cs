using System.IO;
using UnityEditor;
using UnityEngine;
using Urd.Services;
using Urd.Utils;

namespace Urd.Editor
{
    public class InitialConfig : MonoBehaviour
    {
        private const string CONFIG_FOLDER = "Configurations";

        private static string Folder => $"{Application.dataPath}/{CONFIG_FOLDER}";
        private static string RelativeFolder => $"Assets/{CONFIG_FOLDER}";
        
        
        [MenuItem("Urd/Initial Configuration")]
        public static void CreateInitialConfig()
        {
            CreateConfigFolder();
            CreateServiceLocatorConfig();
        }

        private static void CreateServiceLocatorConfig()
        {
            var serviceLocatorConfig = ScriptableObject.CreateInstance<ServiceLocatorConfig>();

            serviceLocatorConfig.FillWithAllServices();
            
            AssetDatabase.CreateAsset(serviceLocatorConfig, $"{RelativeFolder}/ServiceLocatorConfig.asset");
            AssetDatabase.SaveAssets();
        }

        private static void CreateConfigFolder()
        {
            if (!Directory.Exists(Folder))
            {
                Directory.CreateDirectory(Folder);
            }
        }
    }
}
