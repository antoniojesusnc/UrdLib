using UnityEngine;

namespace BrenaBurger
{
    [ExecuteAlways]
    public class HorizontalLayoutWorld : ChildLayoutWorld
    {
        protected override Vector3 Direction => Vector3.right;
    }
}
