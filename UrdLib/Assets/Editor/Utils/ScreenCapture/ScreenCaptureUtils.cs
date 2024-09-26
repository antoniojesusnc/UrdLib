using System;
using UnityEngine;
using UnityEngine.Windows;

namespace Urd.Utils
{
    public class ScreenCaptureUtils 
    {
        public static void TakeScreen(string name = null)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = GetName();
            }

            var path = Application.persistentDataPath + "/Screenshots/";
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            ScreenCapture.CaptureScreenshot(path+name);
            
            Debug.Log($"Image Saved in {path+name}");
        }

        private static string GetName()
        {
            return DateTime.UtcNow.ToString()
                           .Replace(" ", "_")
                           .Replace(":", "-")
                           .Replace("/","-")+".png";
        }
    }
}