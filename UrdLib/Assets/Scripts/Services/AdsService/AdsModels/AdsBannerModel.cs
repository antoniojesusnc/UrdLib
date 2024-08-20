using UnityEngine;

namespace Urd.Services
{
    public class AdsBannerModel 
    {
        public AdsBannerPosition Position { get; private set; }
        public Vector2Int Size { get; private set; }

        public AdsBannerModel() : this(AdsBannerPosition.Top, AdsUtils.StandardsSize) { }

        public AdsBannerModel(AdsBannerPosition position, Vector2Int size)
        {
            Position = position;
            Size = size;
        }
    }
}
