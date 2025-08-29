using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using MyBox;
using Urd.Character;
using Urd.Events;
using Urd.Services;
using UnityEngine;

namespace Urd
{
    [Serializable]
    public class PlayerWallet : IPlayerWallet
    {
        public Dictionary<CharacterAttributeType, AttributeModel> Resources { get; private set; } = new();
        
        public bool Contains(CharacterAttributeType type) => Resources.ContainsKey(type);
        public AttributeModel Get(CharacterAttributeType type) => Resources.ContainsKey(type) ? Resources[type] : null;
        public bool TryGet(CharacterAttributeType type, out AttributeModel attributeModel) => Resources.TryGetValue(type, out attributeModel);
        public void AddResource(AttributeModel resource) => Resources[resource.CharacterAttributeType] = resource;
        public bool TryAddResource(AttributeModel resource) => Resources.TryAdd(resource.CharacterAttributeType, resource);

        public virtual void Dispose()
        {
            Resources.ForEach(resource => resource.Value.Dispose());
            Resources.Clear();
        }
        
        public bool CanAfford(CharacterAttributeType type, double amount)
        {
            if (!Resources.TryGetValue(type, out AttributeModel resource)) 
                return false;
            return resource.Current >= amount;
        }

        public void AddAmount(CharacterAttributeType type, double amount, Vector3 origin = default, bool isFromUI = false)
        {
            if (!Resources.TryGetValue(type, out AttributeModel resource))
            {
                resource = new AttributeModel(type, double.MaxValue, 0, 0);
                AddResource(resource);                
            }

            resource.Add(amount);

            StaticServiceLocator.Get<IEventBusService>().Send(new OnWalletAttributeChangedEvent(type, amount, origin, isFromUI));
        }
        
        public void SubtractAmount(CharacterAttributeType type, double amount, Vector3 origin = default, bool isFromUI = false)
        {
            if (!Resources.TryGetValue(type, out AttributeModel resource)) 
                return;
            
            resource.Deduct(amount);
            StaticServiceLocator.Get<IEventBusService>().Send(new OnWalletAttributeChangedEvent(type, -amount, origin, isFromUI));
        }
        
        public string GetResourceString(CharacterAttributeType type)
        {
            return !Resources.TryGetValue(type, out AttributeModel resource) 
                ? string.Empty : resource.Current.ToShortFormat();
        }
    }
}