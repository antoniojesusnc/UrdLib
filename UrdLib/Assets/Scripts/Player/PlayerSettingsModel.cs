using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerSettingsModel : IDisposable
{
    [SerializeField, JsonProperty("music_toggle")]
    public bool MusicToggle { get; set; }
    [SerializeField, JsonProperty("sfx")]
    public bool SfxToggle { get; set; }
    [SerializeField, JsonProperty("haptic")]
    public bool HapticToggle { get; set; }
    [SerializeField, JsonProperty("notifications")]
    public bool NotificationToggle { get; set; }
    
    public PlayerSettingsModel()
    {
        
    }
    
    public void Dispose()
    {
        
    }
}
