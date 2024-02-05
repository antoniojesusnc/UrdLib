using System;
using System.Collections.Generic;
using UnityEngine;
using Urd.Services;

namespace Urd.Utils
{
    public abstract class FsmController<TController, T> : IDisposable
        where TController : FsmController<TController, T>
        where T : System.IComparable
    {
        public bool EnableLog = false;
        
        public bool CheckAllPossibleTransitions { get; set; }
        
        private List<FsmState<TController, T>> _allStates;
        private List<FsmTransition<TController, T>> _allTransitions;
        
        private FsmState<TController, T> _currentState;
        private List<FsmTransition<TController, T>> _currentStateTransitions;

        private FsmTransition<TController, T> _transitionToNextState;

        public event Action<FsmState<TController, T>> OnFSMStateChanged;

        private IClockService _clockService;
        
        public FsmController()
        {
            _allStates = new List<FsmState<TController, T>>();
            _allTransitions = new List<FsmTransition<TController, T>>();

            _clockService = StaticServiceLocator.Get<IClockService>();
            
            CreateFSM();
        }

        protected abstract void CreateFSM();
        
        public virtual void InitFSM()
        {
            if (_currentState == null)
            {
                Debug.LogWarning("FirstState not definded");
                return;
            }

            _clockService.SubscribeToUpdate(CustomUpdate);
            _currentState.OnActivate();
        }
        public virtual void Dispose()
        {
            _currentState.OnDeactivate();
            _currentState = null;
            _currentStateTransitions = null;
            _transitionToNextState = null;
            
            _clockService.UnSubscribeToUpdate(CustomUpdate);
            _clockService = null;
            
            _allStates?.ForEach(state => state.Dispose());
            _allStates?.Clear();
            _allStates = null;
            _allTransitions?.ForEach(transitions => transitions.Dispose());
            _allTransitions?.Clear();
            _allTransitions = null;
            OnFSMStateChanged = null;

        }

        private void CustomUpdate(float deltaTime)
        {
            if (_currentState == null)
            {
                return;
            }
            
            _currentState.Update(deltaTime);

            _transitionToNextState = null;
            for (int i = _currentStateTransitions.Count - 1; i >= 0; --i)
            {
                if (_currentStateTransitions[i].CanNavigateToNextState())
                {
                    if (_transitionToNextState != null)
                    {
                        Debug.LogWarning("There are more that one possible transition");   
                    }
                    _transitionToNextState = _currentStateTransitions[i];
                    if (!CheckAllPossibleTransitions)
                    {
                        break;
                    }
                }
            }

            if (_transitionToNextState != null)
            {
                ChangeState(_transitionToNextState.NextState);
            }
        }

        public void ForceChangeState(T newState) => ChangeState(newState);
        protected virtual void ChangeState(T newState)
        {
            //Debug.Log($"Transition from {_currentState.State} to {newState}");
            if (_currentState.State.Equals(newState))
            {
                return;
            }
            
            if (_currentState != null)
                _currentState.OnDeactivate();

            _currentState = GetState(newState);
            if (_currentState == null)
            {
                Debug.LogError("The state " + newState.ToString() + "doesnt exist in the FSM");
                OnFSMStateChanged?.Invoke(_currentState);

                return;
            }

            _currentStateTransitions = _currentState.Transitions;
            _currentState.OnActivate();
            
            OnFSMStateChanged?.Invoke(_currentState);
        }

        public void SetFirstState(FsmState<TController, T> firstState)
        {
            _currentState = firstState;
            _currentStateTransitions = _currentState.Transitions;
        }
        
        public List<FsmState<TController, T>> GetAllStates()
        {
            return _allStates;
        }

        public FsmState<TController, T> GetState(T state)
        {
            for (int i = _allStates.Count - 1; i >= 0; --i)
            {
                if (_allStates[i].State.Equals(state))
                {
                    return _allStates[i];
                }
            }

            return null;
        }
        
        public void AddState(FsmState<TController, T> newState)
        {
            _allStates.Add(newState);
        }
        
        public void AddTransition(FsmTransition<TController, T> newTransition)
        {
            _allTransitions.Add(newTransition);
        }
    }
}