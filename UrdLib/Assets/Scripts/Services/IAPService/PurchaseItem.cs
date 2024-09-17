using System;
using UnityEngine.Localization.PropertyVariants;
using UnityEngine.Purchasing;

namespace Urd.Services.IAP
{
    public class PurchaseItem : IDisposable
    {
        public string Id { get; private set; }
        public ProductType ProductType { get; private set; } = ProductType.Consumable;
        public PurchaseEventArgs PurchaseEvent { get; private set; }
        public string Error { get; private set; }

        public bool IsSuccess => PurchaseEvent != null;

        public PurchaseItem(string id) : this(id, ProductType.Consumable)
        {
        }
        public PurchaseItem(string id, ProductType productType)
        {
            Id = id;
            ProductType = productType;
        }
        
        public void Dispose()
        {
            PurchaseEvent = null;
        }

        public void AddPurchaseEventArgs(PurchaseEventArgs purchaseEvent)
        {
            PurchaseEvent = purchaseEvent;
        }

        public void AddPurchaseError(string error)
        {
            Error = error;
        }
    }
}