using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using Urd.Audio;
using Urd.Navigation;
using Urd.Services;

namespace Urd.Editor
{
    [CustomEditor(typeof(AudioConfig))]
    public class AudioConfigCustomEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawAddAllMixersButton();
        }

        private void DrawAddAllMixersButton()
        {
            if (GUILayout.Button("Add All Mixers"))
            {
                var audioMixerGroups = AssetsUtils.GetAllPrefabThatHas<AudioMixerGroup>();
                var audioConfig = target as AudioConfig;
                audioConfig.Mixers.Clear();
                for (int i = 0; i < audioMixerGroups.Count; i++)
                {
                    audioConfig.Mixers.Add(new AudioMixerData((AudioMixerType)(i+1), audioMixerGroups[i]));
                }

                AssetDatabase.SaveAssets();
            }
        }
    }
}
