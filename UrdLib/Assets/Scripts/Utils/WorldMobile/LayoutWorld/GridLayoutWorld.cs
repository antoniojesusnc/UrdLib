using UnityEngine;

namespace Urd.Utils.WorldMobile
{
    [ExecuteAlways]
    public class GridLayoutWorld : ChildLayoutWorld
    {
        private enum GridDirection
        {
            Right,
            Top
        }

        [SerializeField] private GridDirection _gridDirection;
        protected override Vector3 Direction => _gridDirection == GridDirection.Right? Vector3.right : Vector3.down;
        private Vector3 OtherDirection => _gridDirection == GridDirection.Right? Vector3.down : Vector3.right;
        
        [SerializeField] 
        private int _elemntsPerColOrRow;
        
        protected override void AdjustChildrenSize(Vector3 initialPosition)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                child.transform.position = 
                    initialPosition + Direction *_childSize * ((i)%_elemntsPerColOrRow) + Direction*_space*((i)%_elemntsPerColOrRow)
                    + OtherDirection *_childSize * ((i)/_elemntsPerColOrRow) + OtherDirection*_space*((i)/_elemntsPerColOrRow);
            }
        }
    }
}