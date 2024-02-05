using TMPro;
using UnityEngine;

namespace Urd.UI
{
    [CreateAssetMenu(fileName = "StyleConfig", menuName = "Urd/UI/UI Style Config", order = 1)]
    public class UIStyleConfig : ScriptableObject
    {
        [field: SerializeField]
        public Color MainBackgroundColor { get; private set; }
        
        [field: SerializeField]
        public TMP_FontAsset MainFont { get; private set; }
        
        [field: SerializeField]
        public TMP_FontAsset SecondaryFont { get; private set; }
    }
}