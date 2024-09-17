using System;
using System.Collections.Generic;
using RubberDuck.Gameplay;
using UnityEngine;
using Urd.Services.IAP;

namespace Urd.Services
{
    [Serializable]
    public class IAPService : BaseService, IIAPService
    {        
        public override int LoadPriority => 20;

        [SerializeReference, SubclassSelector] 
        public IIAPServiceProvider _provider;

        public override void Init()
        {
            base.Init();
        }

        public void AddPurchaseItems(List<PurchaseItem> items,  Action<bool, List<string>> onAddPurchaseItems)
        {
            _provider.Init(items, (success, idList) => OnInitialized(success, idList, onAddPurchaseItems));
        }

        private void OnInitialized(bool success, List<string> idList, Action<bool, List<string>> onAddPurchaseItems)
        {
            Debug.Log($"IAPService: {success}");
            onAddPurchaseItems?.Invoke(success, idList);
        }

        public void Purchase(PurchaseItem purchaseItem, Action<PurchaseItem> onPurchase = null)
        {
            _provider.Purchase(purchaseItem, onPurchase);
        }

        public string GetPriceOf(IStoreItemModel storeItemModel)
        {
            return _provider.GetPriceOf(storeItemModel);
        }
    }
}