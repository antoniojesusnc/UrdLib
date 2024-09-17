using RubberDuck.UI;
using UnityEngine;
using UnityEngine.Localization;
using Urd.Services.IAP;
using Urd.Utils;

namespace RubberDuck.Config
{
    [CreateAssetMenu(fileName = "StoreItem Config", menuName = "RubberDuck/StoreItem/New StoreItem", order = 1)]
    public class StoreItemConfig : ScriptableObject
    {
        [field: SerializeField]
        public string StoreItemId { get; private set; }
        [field: SerializeField]
        public LocalizedString Name { get; private set; }
        [field: SerializeField]
        public LocalizedString Description { get; private set; }
        
        [field: SerializeField]
        public StoreItemTypes Type { get; private set; }
        
        [field: SerializeField]
        public PurchaseTypes PurchaseType { get; private set; }
        
        [field: SerializeField]
        public StoreSectionTypes Section { get; private set; }
        
        [field: SerializeField]
        public int DurationMinutes { get; private set; }
        
        [field: SerializeField, PreviewSprite]
        public Sprite Image { get; private set; }
        
        [field: SerializeField]
        public string Price { get; private set; }
        [field: SerializeField]
        public bool ShowInfo { get; private set; }
        
        [field: SerializeField]
        public string RewardPiece { get; private set; }
        
        [field: SerializeField]
        public string RewardChest { get; private set; }
        
        [field: SerializeField]
        public string AndroidProductId { get; private set; }
        [field: SerializeField]
        public string IosProductID { get; private set; }


        public void Hydrate(string id, LocalizedString name, LocalizedString description, StoreItemTypes type, PurchaseTypes purchaseType, StoreSectionTypes section, int durationMinutes,
            Sprite image, string price, bool showInfo, string rewardPiece, string rewardChest, string androidProductId,
            string iosProductID)
        {
            StoreItemId = id;
            Name = name;
            Description = description;
            Type = type;
            PurchaseType = purchaseType;
            Section = section;
            DurationMinutes = durationMinutes;
            Image = image;
            Price = price;
            ShowInfo = showInfo;
            RewardPiece = rewardPiece;
            RewardChest = rewardChest;
            AndroidProductId = androidProductId;
            IosProductID = iosProductID;
        }
    }
}