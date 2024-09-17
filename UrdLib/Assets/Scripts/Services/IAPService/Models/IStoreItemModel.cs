using System;
using RubberDuck.Config;
using RubberDuck.UI;
using UnityEngine;
using UnityEngine.Localization;
using Urd.Services.IAP;

namespace RubberDuck.Gameplay
{
    public interface IStoreItemModel : IDisposable
    {
        LocalizedString Name { get; }
        LocalizedString Description  { get; }
        string StoreItemId  { get; }
        StoreItemTypes Type  { get; }
        PurchaseTypes PurchaseType  { get; }
        Sprite Sprite { get; }
        string Price { get; }
        int DurationMinutes { get; }
        DateTime UnlockDateTime { get; }
        bool IsPurchased { get; }
        string RewardPieces { get; }
        string RewardChest { get; }
        bool IsInCoolDown { get; }
        bool IsFree { get; }
        bool ShowInfo { get; }
        string StoreId { get; }

        public event Action OnClaimed;
        void SetConfig(StoreItemConfig storeItemConfig);

        void Claim(DateTime now);
        void SetPurchased(bool isPurchased);
        string GetPriceText();
        LocalizedString GetDescription();
    }
}
