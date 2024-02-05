using UnityEngine;

namespace Urd.Utils
{
    public class LayerUtils
    {
        public static LayerMask Interactable => 1 << LayerMask.NameToLayer("Interactable");
    }
}
