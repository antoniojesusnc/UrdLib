using System;
using System.Collections.Generic;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Urd.Editor.Utils
{
    public class EditorSearchByFileName
    {
        public const char DEFAULT_SPLIT_CHARACTER = ',';

        public static T GetFile<T>(string fileName) where T : Object
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            string toSearch = $"t:{typeof(T).Name} {fileName}";
            var files = AssetDatabase.FindAssets(toSearch);
            if (files == null || files.Length <= 0)
            {
                return null;
            }

            for (int i = 0; i < files.Length; i++)
            {
                var searched = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(files[i]));
                if (string.Equals(searched.name.Trim(), fileName.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return searched;
                }
            }

            return null;
        }

        public static List<T> GetFiles<T>(string files, char splitCharacter = DEFAULT_SPLIT_CHARACTER) where T : Object 
        {
            List<T> resultList = new List<T>();
            var filesSplitted = files.Split(splitCharacter);
            for (int i = 0; i < filesSplitted.Length; i++)
            {
                var result = GetFile<T>(filesSplitted[i].Trim());
                if (result != null)
                {
                    resultList.Add(result);
                }
            }
            
            return resultList;
        }
    }
}
