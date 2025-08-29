#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Urd.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(EventTrigger))]
    public class HoldableButton : Button
    {
        [SerializeField]
        private float _holdTime = 0.2f;
        [SerializeField]
        private float _holdMaxSpeedTime = 0.1f;
        [SerializeField]
        private float _timeToReachMaxSpeed = 1f;

        private bool _actionDone;
        private bool _holding;
        private float _beginClick;
        
        protected override void Awake()
        {
            base.Awake();
            if (transform.parent == null)
                return;
            
            gameObject.name = gameObject.name.Replace("Holdable", "");
            foreach (Transform sibling in transform.parent)
            {
                if(sibling == transform)
                    continue;
                
                if(sibling.name == gameObject.name && !sibling.gameObject.activeSelf)
                    Destroy(sibling.gameObject);
            }
        }

        protected override void Start()
        {
            base.Start();

            
            Subscribe();
        }

        private void Subscribe()
        {
            // trigger Down
            AddTrigger(EventTriggerType.PointerDown, OnHoldDown);
            AddTrigger(EventTriggerType.PointerUp, OnHoldUp);
        }
        private void AddTrigger(EventTriggerType eventTrigger, UnityAction<BaseEventData> callback)
        {
            var triggers = GetComponent<EventTrigger>().triggers;
            var trigger = triggers.Find(entry => entry.eventID == eventTrigger);
            if (trigger == null)
            {
                trigger = new EventTrigger.Entry()
                {
                    eventID = eventTrigger,
                };
                triggers.Add(trigger);
            }

            for (int i = 0; i < trigger.callback.GetPersistentEventCount(); i++)
            {
                var actualCallbackName = trigger.callback.GetPersistentMethodName(i);
                if (actualCallbackName != null && actualCallbackName == callback.Method.Name)
                {
                    return;
                }
            }

            #if UNITY_EDITOR
            UnityEventTools.AddPersistentListener(trigger.callback, callback);
            #endif
        }

        private void OnHoldDown(BaseEventData arg0)
        {
            _holding = true;
            _beginClick = Time.realtimeSinceStartup;
            _actionDone = false;
            Invoke(nameof(DoAction), GetHoldTime());
        }

        private float GetHoldTime()
        {
            return Mathf.Lerp(_holdTime, _holdMaxSpeedTime, (Time.realtimeSinceStartup-_beginClick)/_timeToReachMaxSpeed);
        }

        private void OnHoldUp(BaseEventData arg0)
        {
            _holding = false;
            CancelInvoke();
            if (!_actionDone)
            {
                DoAction();
            }
        }
        
        public void DoAction()
        {
            _actionDone = true;
            base.OnPointerClick(new PointerEventData(EventSystem.current));
            if (_holding)
            {
                Invoke(nameof(DoAction), GetHoldTime());
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnHoldUp(null);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            
        }


        #if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            Subscribe();
        }
        #endif
    }
}