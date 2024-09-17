using System;
using System.Collections.Generic;
using RubberDuck.Gameplay;
using UnityEngine.Purchasing;

namespace Urd.Services.IAP
{
    public interface IIAPServiceProvider
    {
        void Init(List<PurchaseItem> items, Action<bool, List<string>> onInitialized = null);
        void Purchase(PurchaseItem purchaseItem, Action<PurchaseItem> onPurchase = null);
        string GetPriceOf(IStoreItemModel storeItemModel);
    }
}
