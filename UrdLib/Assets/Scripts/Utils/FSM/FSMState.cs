using System;
using System.Collections.Generic;

namespace Urd.Utils
{
    public abstract class FsmState<TController, T> : IDisposable
        where TController : FsmController<TController, T>
        where T : System.IComparable
    {
        protected TController Controller { get; private set; }
        public List<FsmTransition<TController, T>> Transitions { get; private set; }
        public T State { get; private set; }

        public FsmState(TController controller, T state)
        {
            Transitions = new List<FsmTransition<TController, T>>();
            Controller = controller;
            State = state;
            
            Controller.AddState(this);
        }

        public virtual void Dispose()
        {
            Transitions?.Clear();
            Transitions = null;
            
            Controller = null;
        }
        
        public virtual void OnActivate()
        {
        }

        public virtual void Update(float dt)
        {

        }

        public virtual void OnDeactivate()
        {

        }


        // Transition methods
        public void AddTransition(FsmTransition<TController, T> newTransition)
        {
            AddTransitions(newTransition);
        }
        
        public void AddTransitions(params FsmTransition<TController, T>[] newTransitions)
        {
            Transitions.AddRange(newTransitions);
        }

        public void RemoveTransition(FsmTransition<TController, T> removeTransition)
        {
            Transitions.Remove(removeTransition);
        }

        public void RemoveTransitionTo(T removeTransition)
        {
            for (int i = Transitions.Count; i >= 0; --i)
            {
                if (Transitions[i].NextState.CompareTo(removeTransition) == 0)
                {
                    Transitions.RemoveAt(i);
                    return;
                }
            }
        }
    }
}