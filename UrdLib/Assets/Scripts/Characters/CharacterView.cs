using UnityEngine;

namespace Urd
{
    public class CharacterView<T> :  MonoBehaviourEventObservable
    {
        [SerializeField] protected SpriteRenderer _mainImage;

        public T Model { get; private set; }

        protected virtual void Awake()
        {
            
        }

        public virtual void SetModel(T enemyModel)
        {
            Model = enemyModel;

            SubscribeEvents();

            UpdateData();
        }

        public override void SubscribeEvents() { }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            UnsubscribeEvents();
        }

        public override void UnsubscribeEvents() { }

        protected virtual void UpdateData()
        {
            
        }

        public virtual void SetAsDeath()
        {

        }
    }
}