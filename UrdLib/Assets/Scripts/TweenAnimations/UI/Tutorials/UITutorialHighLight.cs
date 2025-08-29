using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Coffee.UIEffects;
using DG.Tweening;
using MyBox;
using Newtonsoft.Json.Serialization;
using Urd.Events;
using Urd.Services;
using Urd.Tutorial;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using Urd.Gameplay;
using Image = UnityEngine.UI.Image;

namespace Urd
{
    public class UITutorialHighLight : MonoBehaviourEventObservable,
        IEventBusObservable<OnBeginTutorialEvent>,
        IEventBusObservable<OnFinishTutorialEvent>
    {
        [SerializeField] public RectTransform _iris;
        [SerializeField] public TextMeshProUGUI _message;
        [SerializeField] public Image _image;
        [SerializeField] public Image _screen;
        
        [SerializeField] public Image _irisImage;

        [SerializeField] private RectTransform _clickArea;

        private TutorialStepHighlightAreaMessage _tutorial;
        private GameObject _objective;
        private Button _button;
        private EventTrigger _eventTrigger;
        protected override void Start()
        {
            base.Start();
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy && _objective == null || !_objective.activeInHierarchy)
            {
                UnSubscribeToClickInButton();
                FinishHighLight();
            }
        }

        private void OnDisable()
        {
            UnSubscribeToClickInButton();
        }

        private async void BeginHighLight(TutorialStepHighlightAreaMessage tutorialEvent)
        {
            try
            {
                bool gameJustStarted = Time.time < 1;
                if (gameJustStarted) 
                    await Task.Delay(1000);
            
                var tutorialModule = StaticServiceLocator.Get<IGamePlayService>().GetModule<GamePlayTutorialModule>();
                await Task.Delay(Mathf.RoundToInt(f: 1000 * tutorialEvent.DelayToShow));
                if(tutorialModule.CurrentTutorialStep != tutorialEvent ||
                   tutorialModule.IsTutorialDone(tutorialEvent) || !tutorialEvent.Dependencies.AreMet())
                {
                    tutorialModule.RevaluateAllTutorialSteps();
                    return;
                }

                gameObject.SetActive(true);
                _tutorial = tutorialEvent;

                _screen.gameObject.SetActive(tutorialEvent.HighlightArea);
                _iris.gameObject.SetActive(tutorialEvent.HighlightArea);
                _irisImage.raycastTarget = tutorialEvent.HighlightArea;
            
                _objective = GameObject.Find(tutorialEvent.UiObjectToFocusToSearch);
            
                if (_objective == null || !_objective.activeInHierarchy)
                {   
                    Debug.LogWarning($"No objective found{tutorialEvent.UiObjectToFocusToSearch}");
                    gameObject.SetActive(false);
                    return;
                }

                Vector3 position = _objective.GetComponent<RectTransform>() == null
                    ? Camera.main.WorldToScreenPoint(_objective.transform.position)
                    : _objective.transform.position;
                position += tutorialEvent.UiObjectToFocusToSearchOffset * _iris.localScale.x;
            
                if (tutorialEvent.DetectClicks)
                {
                    UnSubscribeToClickInButton();
                    AssignButtonAndTrigger(_objective);
                }

                _iris.transform.position = position;
                _iris.sizeDelta = tutorialEvent.AreaSize;

                _clickArea.transform.position = position;
                _clickArea.sizeDelta = tutorialEvent.AreaSize * 1.5f;
                _clickArea.gameObject.SetActive(tutorialEvent.HighlightArea);

                _message.gameObject.SetActive(tutorialEvent.HasMessage);
                _message.gameObject.SetActive(tutorialEvent.HasMessage);
                if (tutorialEvent.HasMessage)
                {
                    _message.text = tutorialEvent.Message;
                    _message.fontSize = tutorialEvent.MessageFontSize;
                    _message.transform.position = _iris.transform.position;
                    _message.transform.position += tutorialEvent.MessageOffset * _message.transform.lossyScale.x;
                }

                _image.gameObject.SetActive(tutorialEvent.HasImage);
                if (!tutorialEvent.HasImage) 
                    return;
                _image.enabled = true;
                _image.sprite = tutorialEvent.Image;
                _image.rectTransform.sizeDelta = tutorialEvent.ImageSize;
                _image.transform.parent.position = _iris.transform.position;
                _image.transform.parent.position += tutorialEvent.ImageOffset * _message.transform.lossyScale.x;
                _image.transform.parent.rotation = Quaternion.Euler(tutorialEvent.ImageRotation);

                _image.transform.localRotation = Quaternion.Euler(0, 0, tutorialEvent.FlipX ? 180 : 90);

                if (tutorialEvent.FlipX || tutorialEvent.FlipY)
                {
                    _image.GetOrAddComponent<UIFlip>().horizontal = tutorialEvent.FlipX;
                    _image.GetOrAddComponent<UIFlip>().vertical = tutorialEvent.FlipY;
                }

                _image.transform.DOScale(Vector3.one, duration: 0.4f)
                    .SetEase(Ease.OutBack)
                    .From(Vector3.zero)
                    .SetLink(_image.gameObject);
            }
            catch (Exception e)
            {
                Debug.LogError($"Error in UITutorialHighLight: {e.Message}\n{e.StackTrace}");
                gameObject.SetActive(false);
                UnSubscribeToClickInButton();
            }
        }

        private void FinishHighLight()
        {
            gameObject.SetActive(false);

            UIFlip uiFlip = _image.GetComponent<UIFlip>();
            if (uiFlip != null) 
                Destroy(uiFlip);
        }

        private void AssignButtonAndTrigger(GameObject objective)
        {
            _button = objective.GetComponentInChildren<Button>();
            if (_button != null) 
                _button.onClick?.AddListener(OnClick);
            
            _eventTrigger = objective.GetComponentInChildren<EventTrigger>();
            if (_eventTrigger == null) 
                return;
            EventTrigger.Entry trigger = _eventTrigger.triggers.Find(MatchPointerDown);
            if (trigger != null)
                return;
            trigger = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
            _eventTrigger.triggers.Add(trigger);
        }

        private void UnSubscribeToClickInButton()
        {
            _button?.onClick.RemoveListener(OnClick);
            _eventTrigger?.triggers.Find(entry => entry.eventID == EventTriggerType.PointerDown)?.callback.RemoveListener(OnClick);
        }
        private void OnClick(BaseEventData arg0)
        {
            UnSubscribeToClickInButton();
            OnClick();
        }

        public void OnNewEvent(OnBeginTutorialEvent newEvent)
        {
            if (newEvent.TutorialStep != _tutorial) 
                FinishHighLight();

            if (newEvent.TutorialStep is not TutorialStepHighlightAreaMessage tutorialEvent)
                return;
            
            BeginHighLight(tutorialEvent);
        }

        public void OnClick()
        {
            if (_button != null)
            {
                _button.onClick?.RemoveListener(OnClick);
                _button.onClick?.Invoke();
            }
            if (_eventTrigger != null)
            {
                EventTrigger.Entry trigger = _eventTrigger.triggers.Find(MatchPointerDown);
                BaseEventData eventData = new PointerEventData(EventSystem.current);
                trigger.callback.Invoke(eventData);
            }

            _tutorial.OnClickInArea();
        }

        public void OnNewEvent(OnFinishTutorialEvent newEvent)
        {
            FinishHighLight();
        }
        
        private bool MatchPointerDown(EventTrigger.Entry entry) => entry.eventID == EventTriggerType.PointerDown;
    }
}