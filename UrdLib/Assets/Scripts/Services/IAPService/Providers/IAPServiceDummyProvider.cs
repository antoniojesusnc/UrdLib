using System;
using System.Collections.Generic;
using DG.Tweening;
using RubberDuck.Gameplay;
using UnityEngine;
using UnityEngine.Purchasing;

namespace Urd.Services.IAP
{
    [Serializable]
    public class IAPServiceDummyProvider : IIAPServiceProvider
    {
        [SerializeField] private bool _successInit = true;
        [SerializeField] private bool _successPurchase = true;

        public void Init(List<PurchaseItem> items, Action<bool, List<string>> onInitialized = null)
        {
            DOVirtual.DelayedCall(0.1f, () => onInitialized?.Invoke(_successInit, items.ConvertAll(item => item.Id)));
        }

        public void Purchase(PurchaseItem purchaseItem, Action<PurchaseItem> onPurchase = null)
        {
            if (_successPurchase)
            {
                purchaseItem.AddPurchaseEventArgs(default);
            }

            DOVirtual.DelayedCall(0.1f, () => onPurchase?.Invoke(purchaseItem));
        }

        public string GetPriceOf(IStoreItemModel storeItemModel)
        {
            
            if (int.TryParse(storeItemModel.Price, out var amount) && amount > 0)
            {
                return $"{(amount / 100f).ToString("#0.00")} €";
            }

            return storeItemModel.Price;
        }
    }
}
