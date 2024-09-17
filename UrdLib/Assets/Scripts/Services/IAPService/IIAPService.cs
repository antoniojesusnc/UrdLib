using System;
using System.Collections.Generic;
using RubberDuck.Gameplay;
using Urd.Services.IAP;

namespace Urd.Services
{
    public interface IIAPService : IBaseService
    {
        void AddPurchaseItems(List<PurchaseItem> items, Action<bool, List<string>> onAddPurchaseItems);
        void Purchase(PurchaseItem purchaseItem, Action<PurchaseItem> onPurchase = null);
        string GetPriceOf(IStoreItemModel storeItemPiecesModel);
    }
}