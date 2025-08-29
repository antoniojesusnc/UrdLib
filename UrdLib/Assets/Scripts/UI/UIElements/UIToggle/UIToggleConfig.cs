using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIToggleConfig : ScriptableObject
{
    [field: Header("On")]
    [field: SerializeField]
    public Color OnBackgroundColor { get; private set; }
    [field: SerializeField]
    public Color OnTextColor { get; private set; }
    
    [field: Header("Off")]
    [field: SerializeField]
    public Color OffBackgroundColor { get; private set; }
    [field: SerializeField]
    public Color OffTextColor { get; private set; }
}
