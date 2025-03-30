using System;
using System.Collections.Generic;
using System.IO;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using Urd.Utils;

namespace RubberDuck.Editor
{
    public class GoogleSheetDataCreator
    {
        private static char CSV_SEPARATOR = '\t';
        private static string RELATIVE_FOLDER_FORMAT => Application.dataPath+"/Configurations/{0}";
        private static string RELATIVE_FILE_NAME_FORMAT => "Assets/Configurations/{0}/{1}.asset";

        public static void AddButton<T>(string googleSheetId, UnityEditor.Editor file, string urlSheetData, List<T> list, Func<string[], T> parser, string folder, bool removePreviousFilesInFolder)where T : ScriptableObject
        {
            if (GUILayout.Button("Update Data From Google Sheet"))
            {
                CreateData(
                    googleSheetId,
                    urlSheetData,
                    list,
                    parser,
                    folder,
                    removePreviousFilesInFolder);
            }
            
            GUILayout.Space(3);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(3);

            EditorUtility.SetDirty(file.target);
        }

        public static void CreateData<T>(string googleSheetId, string urlSheetData, List<T> list, Func<string[], T> parser, string folder, bool removePreviousFilesInFolder) where T : ScriptableObject
        {
            CreateFolderIfNotExist(folder);
            
            if (removePreviousFilesInFolder)
            {
                RemovePreviousFilesInFolder(folder, list);
            }
            
            var googleSheetLoader = new GoogleSheetLoader();
            EditorCoroutineUtility.StartCoroutineOwnerless(
                googleSheetLoader.LoadTextDataCo(
                    googleSheetId,
                    urlSheetData,
                    (success, csvData) =>
                        OnDataRead(success, csvData, list, parser, folder)));
        }

        private static void RemovePreviousFilesInFolder<T>(string folder, List<T> list)
        {
            var path = string.Format(RELATIVE_FOLDER_FORMAT, folder);
            var filesPath = Directory.GetFiles(path);
            for (int i = 0; i < filesPath.Length; i++)
            {
                File.Delete(filesPath[i]);
            }
            list.Clear();
            AssetDatabase.Refresh();
        }

        private static void CreateFolderIfNotExist(string folder)
        {
            var path = string.Format(RELATIVE_FOLDER_FORMAT, folder);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static void OnDataRead<T>(bool success, string[] csvData, List<T> list, Func<string[], T> parser, string folder) where T : ScriptableObject
        {
            CreateDataFromCSV(csvData, list, parser, folder);
        }

        private static void CreateDataFromCSV<T>(string[] csvData, List<T> list, Func<string[], T> parser, string folder) where T : ScriptableObject
        {
            for (int i = 1; i < csvData.Length; i++)
            {
                var config = parser?.Invoke(csvData[i].Trim().Split(CSV_SEPARATOR));
                if (config == null)
                {
                    continue;
                }
                AssetDatabase.CreateAsset(config, string.Format(RELATIVE_FILE_NAME_FORMAT, folder, config.name));
                list.Add(config);
            }

            AssetDatabase.SaveAssets();
        }
    }
}