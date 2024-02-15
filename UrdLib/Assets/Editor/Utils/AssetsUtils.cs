using System.Collections.Generic;
using System.Linq;
using DG.DemiEditor;
using UnityEditor;
using UnityEngine;

namespace Urd.Editor
{
    public class AssetsUtils
    {
        public static string[] GetAllPrefabs()
        {
            string[] temp = AssetDatabase.GetAllAssetPaths();
            List<string> result = new List<string>();
            foreach (string s in temp)
            {
                if (s.Contains(".prefab")) result.Add(s);
            }

            return result.ToArray();
        }
        public static List<T> GetAllPrefabThatHas<T>() where T : Object
        {
             List<T> objects = new List<T>();

            var resources = Resources.FindObjectsOfTypeAll<T>();
            if (resources != null)
            {
                objects.AddRange(resources.ToList().FindAll(resource => !resource.name.IsNullOrEmpty()));
            }
            return objects;
        }
    }
}