using System;
using MyBox;
using UnityEngine;
using UnityEngine.UI;

namespace Urd.UI
{
    [ExecuteAlways]
    public class GridLayoutAutoResize : GridLayoutGroup
    {
        private const float REFRESH_TIME = 1f;
        
        [SerializeField] 
        private bool _autoResizeX;
        [SerializeField] 
        private bool _autoResizeY;

        private float _timestamp = 0;
        
        protected override void Awake()
        {
            base.Awake();
            if (_autoResizeX || _autoResizeY)
            {
                Resize();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if (_autoResizeX || _autoResizeY)
            {
                Resize();
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            base.OnRectTransformDimensionsChange();
            Resize();
        }

        private void LateUpdate()
        {
            if (!_autoResizeX && !_autoResizeY)
            {
                return;
            }
            
            if (_timestamp <= 0)
            {
                _timestamp = REFRESH_TIME;
                Resize();
            }            
        }

        [ButtonMethod]
        public void Resize()
        {
            if (_autoResizeX && constraint == Constraint.FixedColumnCount)
            {
                var rect = GetComponent<RectTransform>().rect;
                var xSize = rect.size.x - padding.left - padding.right - spacing.x * (constraintCount - 1);
                xSize /= constraintCount;
                Vector2 newSize = cellSize;
                if (_autoResizeX)
                {
                    newSize = new Vector2(xSize, newSize.y);
                }

                if (_autoResizeY)
                {
                    newSize = new Vector2(newSize.x, xSize);
                }

                cellSize = newSize;
            }
            
            if (_autoResizeY && constraint == Constraint.FixedRowCount)
            {
                var rect = GetComponent<RectTransform>().rect;
                var ySize = rect.size.y - padding.top - padding.bottom - spacing.y * (constraintCount - 1); 
                cellSize = new Vector2(ySize, ySize);
            }
            
            LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
            CalculateLayoutInputHorizontal();
            CalculateLayoutInputVertical();
            SetLayoutHorizontal();
            SetLayoutVertical();
        }
    }
}