using System;
using System.Collections;
using System.Linq;
using Urd;
using Urd.Events;
using Urd.Services;
using UnityEngine;
using Urd.Gameplay;
using Object = UnityEngine.Object;

namespace Urd.Tutorial
{
    [Serializable]
    public class GamePlayTutorialModule : GamePlayModule
    {
        private const string TUTORIAL_KEY_FORMAT = "Tutorial_{0}";
        public const string IS_TUTORIAL_DONE = "IS_TUTORIAL_DONE";
        
        [field: SerializeField] 
        public GamePlayTutorialModuleConfig Config { get; private set; }
        
        [NonSerialized] public bool _isTutorialActive;
        
        [field: NonSerialized] public ITutorialStep CurrentTutorialStep {get; private set;}
        
        public Canvas TutorialCanvas { get; private set; }

        public override void Init()
        {
            base.Init();
            
            TutorialCanvas = Object.Instantiate(Config.CanvasPrefab);
            Object.DontDestroyOnLoad(TutorialCanvas.gameObject);

            if (Config == null || !Config.EnableTutorial)
                return;
            InitSteps();
        }

        public override void Dispose()
        {
            base.Dispose();
            CurrentTutorialStep?.Dispose();
        }

        private void InitSteps()
        {
            foreach (GamePlayTutorialStepConfig tutorialConfig in Config.TutorialSteps.Where(NotCompleted))
            {
                foreach (TutorialDependency dependency in tutorialConfig.TutorialStep.Dependencies)
                {
                    dependency.TutorialStep = tutorialConfig.TutorialStep;
                    dependency.RemoveRevaluationListeners();
                    dependency.AddRevaluationListeners();
                }
            }

            return;

            bool NotCompleted(GamePlayTutorialStepConfig tutorialConfig) 
                => !IsTutorialDone(tutorialConfig.TutorialStep);
        }

        public override IEnumerator BeginGameCoroutine()
        {
            yield return base.BeginGameCoroutine();
            
            if (Config == null || !Config.EnableTutorial)
                yield break;
            
            bool isTutorialDone = StaticServiceLocator.Get<IGamePlayService>().GetPlayerModel<IPlayerModel>().TutorialsCompleted.Contains(IS_TUTORIAL_DONE);
            //bool isTutorialDone = _saveLoadService.Load(IS_TUTORIAL_DONE, false);
            if (Config.TutorialSteps.Count > 0 && !isTutorialDone) 
                BeginTutorial();
        }

        private void BeginTutorial()
        {
            if (_isTutorialActive)
                return;
            
            _isTutorialActive = true;
            RevaluateAllTutorialSteps();
        }

        private bool CanBeginTutorialStep(ITutorialStep tutorialStep)
        {
            if (tutorialStep == null)
                return false;
            if (!tutorialStep.IsActive)
                return false;
            if (CurrentTutorialStep != null || CurrentTutorialStep == tutorialStep)
                return false;
            if(!tutorialStep.Dependencies.AreMet())
                return false;
            return !IsTutorialDone(tutorialStep);
        }

        public void TryBeginTutorialStep(ITutorialStep tutorialStep)
        {
            if (!CanBeginTutorialStep(tutorialStep))
                return;
            
            BeginTutorialStep(tutorialStep);
        }

        public void BeginTutorialStep(ITutorialStep tutorialStep)
        {
            CurrentTutorialStep = tutorialStep;
            tutorialStep.Begin();
            _eventBusService.Send(new OnBeginTutorialEvent(CurrentTutorialStep));
        }
        
        public void CompleteTutorialStep(ITutorialStep tutorialStep)
        {
            foreach (TutorialDependency dependency in tutorialStep.Dependencies) 
                dependency.RemoveRevaluationListeners();
            
            MarkAsCompleted(tutorialStep);
            
            foreach (GamePlayTutorialStepConfig stepConfig in tutorialStep.CompleteTogether) 
                MarkAsCompleted(stepConfig.TutorialStep);
            
            if (CurrentTutorialStep == tutorialStep)
                CurrentTutorialStep = null;
            
            _eventBusService.Send(new OnFinishTutorialEvent(tutorialStep));
            RevaluateAllTutorialSteps();
        }
        
        public void MarkAsCompleted(ITutorialStep tutorialStep)
        {
            StaticServiceLocator.Get<IGamePlayService>().GetPlayerModel<IPlayerModel>().TutorialsCompleted.Add(tutorialStep.Id);
            //_saveLoadService.Save(string.Format(TUTORIAL_KEY_FORMAT, tutorialStep.Id), true);
            FinishTutorialStep(tutorialStep);
        }

        public void FinishTutorialStep(ITutorialStep tutorialStep)
        {
            if (tutorialStep == null)
                return;
            
            tutorialStep.Finish();
            _eventBusService.Send(new OnFinishTutorialEvent(tutorialStep));
        }
        
        public void RestartTutorialStep(ITutorialStep tutorialStep)
        {
            if (Config == null || !Config.EnableTutorial)
                return;
            
            StaticServiceLocator.Get<IGamePlayService>().GetPlayerModel<IPlayerModel>().TutorialsCompleted
                .Remove(tutorialStep.Id);
            //_saveLoadService.Save(string.Format(TUTORIAL_KEY_FORMAT, tutorialStep.Id), false);
            
            FinishTutorialStep(CurrentTutorialStep);
            CurrentTutorialStep = null;
            foreach (TutorialDependency dependency in tutorialStep.Dependencies)
            {
                dependency.TutorialStep = tutorialStep;
                dependency.RemoveRevaluationListeners();
                dependency.AddRevaluationListeners();
            }
            RevaluateAllTutorialSteps();
        }

        public bool IsTutorialDone(ITutorialStep tutorialStep) 
            => StaticServiceLocator.Get<IGamePlayService>().GetPlayerModel<IPlayerModel>().TutorialsCompleted.Contains(tutorialStep.Id);
                //_saveLoadService.Load(string.Format(TUTORIAL_KEY_FORMAT, tutorialStep.Id), false);

        public void RevaluateTutorialStep(ITutorialStep tutorialStep)
        {
            if (CurrentTutorialStep == null)
            {
                if (CanBeginTutorialStep(tutorialStep)) 
                    BeginTutorialStep(tutorialStep);
            }
            else if (CurrentTutorialStep == tutorialStep)
            {
                if (tutorialStep.Dependencies.AreMet()) 
                    return;
                
                RevaluateAllTutorialSteps();
            }
        }
        
        public void RevaluateAllTutorialSteps()
        {
            FinishTutorialStep(CurrentTutorialStep);
            CurrentTutorialStep = null;
            
            foreach (GamePlayTutorialStepConfig config in Config.TutorialSteps)
            {
                if (!CanBeginTutorialStep(config.TutorialStep)) 
                    continue;
                
                BeginTutorialStep(config.TutorialStep);
                break;
            }
            
            _eventBusService.Send(new OnTutorialStepsRevaluationEvent());
        }
    }
}