using System.Collections.Generic;
using Urd.Utils;
using UnityEngine;

namespace Urd.Character
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "Urd/Character/Character Config", order = 1)]
    public class CharacterConfig : ScriptableObject
    {
        [field: Header("Art")]
        [field: SerializeField, PreviewSprite]
        public Sprite IdleImage { get; private set; }
        [field: SerializeField]
        public GameObject CharacterGameObject { get; private set; }
        [field: SerializeField]
        public Vector3 CharacterGameObjectDeltaPosition { get; private set; }
        [field: SerializeField]
        public GameObject EffectWhenDie { get; private set; }

        [field: SerializeReference, SubclassSelector]
        public List<IProgressionData> Progression { get; private set; } = new List<IProgressionData>();
    }
}