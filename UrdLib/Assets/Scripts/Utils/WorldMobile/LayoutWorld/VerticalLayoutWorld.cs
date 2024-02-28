using UnityEngine;

namespace Urd.Utils.WorldMobile
{
    [ExecuteAlways]
    public class VerticalLayoutWorld : ChildLayoutWorld
    {
        protected override Vector3 Direction => Vector3.up;
    }
}
