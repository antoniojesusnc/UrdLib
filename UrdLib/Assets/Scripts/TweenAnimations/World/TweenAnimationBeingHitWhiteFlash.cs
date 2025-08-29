using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Urd.Animation
{
    [CreateAssetMenu(fileName = "TweenAnimationBeingHit", menuName = "Urd/Animations/World/TweenAnimationBeingHitWhiteFlash", order = 1)]
    [Serializable]
    public class TweenAnimationBeingHitWhiteFlash : TweenAnimationBeingHit
    {
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _whiteMaterial;
        public override Tween DoAnimation(List<SpriteRenderer> spriteRenderers)
        {
            for (int i = 0; i < spriteRenderers.Count; i++)
            {
                spriteRenderers[i].material = _whiteMaterial;
            }
            
            var sequence = DOTween.Sequence();

            sequence.AppendInterval(Duration);
            sequence.onComplete += () => RestoreShaders(spriteRenderers);
            return sequence;
        }

        private void RestoreShaders(List<SpriteRenderer> spriteRenderers)
        {
            for (int i = 0; i < spriteRenderers.Count; i++)
            {
                if (spriteRenderers[i] != null)
                {
                    spriteRenderers[i].material = _defaultMaterial;
                }
            }
        }
    }
}
