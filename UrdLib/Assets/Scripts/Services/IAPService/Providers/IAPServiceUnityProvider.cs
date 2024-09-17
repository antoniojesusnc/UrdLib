using System;
using System.Collections.Generic;
using RubberDuck.Gameplay;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Urd.Services.IAP
{
    [Serializable]
    public class IAPServiceUnityProvider : IIAPServiceProvider, IDetailedStoreListener
    {
        private Action<bool, List<string>> _onInitializedCallback;
        private IStoreController _storeController;
        private IExtensionProvider _extensions;
        private PurchaseItem _purchaseItem;
        private Action<PurchaseItem> _onPurchaseCallback;

        public void Init(List<PurchaseItem> items, Action<bool, List<string>> onInitialized)
        {
            _onInitializedCallback = onInitialized;
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                builder.AddProduct(item.Id, item.ProductType);
            }
            
            UnityPurchasing.Initialize(this, builder);
        }

        public void Purchase(PurchaseItem purchaseItem, Action<PurchaseItem> onPurchase)
        {
            _onPurchaseCallback = onPurchase;
            _purchaseItem = purchaseItem;
            _storeController.InitiatePurchase(purchaseItem.Id);
        }

        public string GetPriceOf(IStoreItemModel storeItemModel)
        {
            var product = _storeController.products.WithStoreSpecificID(storeItemModel.StoreItemId);
            return product.metadata.localizedPriceString;
        }

        // unity listeners
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            _storeController = controller;
            _extensions = extensions;
            var allProducts = new List<Product>(controller.products.all);
            _onInitializedCallback?.Invoke(true, allProducts.ConvertAll(product => product.definition.storeSpecificId));
        }
        
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogWarning($"[IAPServiceUnityProvider] OnInitializeFailed {error})");
            _onInitializedCallback?.Invoke(false, null);
            _onInitializedCallback = null;
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            Debug.LogWarning($"[IAPServiceUnityProvider] OnInitializeFailed {error}:{message})");
            _onInitializedCallback?.Invoke(false, null);
            _onInitializedCallback = null;
        }

       public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
       {
           _purchaseItem.AddPurchaseEventArgs(purchaseEvent);
           _onPurchaseCallback?.Invoke(_purchaseItem);
           
            return PurchaseProcessingResult.Complete;
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            _purchaseItem.AddPurchaseError(failureDescription.ToString());
        }
        
        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            _purchaseItem.AddPurchaseError(failureReason.ToString());
        }
    }
}
