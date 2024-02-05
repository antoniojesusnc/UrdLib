using System;

namespace Urd.Utils
{
    public abstract class FsmTransition<TController, StateType> : IDisposable
        where TController : FsmController<TController, StateType>
        where StateType : System.IComparable
    {
        protected TController Controller { get; private set; }
        
        public StateType OriginState{ get; private set; }
        public StateType NextState{ get; private set; }

        public FsmTransition(TController controller, StateType originState, StateType nextState)
        {
            Controller = controller;

            OriginState = originState;
            NextState = nextState;
            
            Controller.AddTransition(this);
        }

        public virtual void Dispose()
        {
            Controller = null;
        } 
        
        public abstract bool CanNavigateToNextState();
    }
}