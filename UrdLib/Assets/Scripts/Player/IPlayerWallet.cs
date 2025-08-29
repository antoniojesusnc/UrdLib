using System;
using System.Collections.Generic;
using Urd.Character;
using UnityEngine;

namespace Urd
{
    public interface IPlayerWallet : IDisposable
    {
        Dictionary<CharacterAttributeType, AttributeModel> Resources { get; }
        bool Contains(CharacterAttributeType type);
        bool TryGet(CharacterAttributeType type, out AttributeModel attributeModel);
        AttributeModel Get(CharacterAttributeType type);
        void AddResource(AttributeModel resource);
        
        bool CanAfford(CharacterAttributeType type, float amount) => CanAfford(type, (double)amount);
        bool CanAfford(CharacterAttributeType type, double amount);
        
        void AddAmount(CharacterAttributeType type, float amount, Vector3 position = default, bool isInUI = false) => AddAmount(type, (double)amount, position, isInUI);
        void AddAmount(CharacterAttributeType type, double amount, Vector3 position = default, bool isInUI = false);
        
        void SubtractAmount(CharacterAttributeType type, float amount, Vector3 position = default, bool isInUI = false) => SubtractAmount(type, (double)amount, position, isInUI);
        void SubtractAmount(CharacterAttributeType type, double amount, Vector3 position = default, bool isInUI = false);
        
        string GetResourceString(CharacterAttributeType type);
    }
}