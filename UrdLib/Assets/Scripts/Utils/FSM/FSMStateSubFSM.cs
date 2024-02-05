
using System;

namespace Urd.Utils
{
    public abstract class FSMStateSubFSM<TControllerSub, TSub, TController, T> : FsmState<TController, T>
        where TControllerSub : FsmController<TControllerSub, TSub>
        where TSub : System.IComparable
        where TController : FsmController<TController, T>
        where T : System.IComparable
    {

        protected TControllerSub _insideFSM;
        
        public FSMStateSubFSM(TController controller, T state) : base(controller, state)
        {
        }

        public override void OnActivate()
        {
            base.OnActivate();

            _insideFSM = Activator.CreateInstance<TControllerSub>();
            _insideFSM.InitFSM();
        }

        public override void OnDeactivate()
        {
            _insideFSM?.Dispose();
            _insideFSM = null;
                
            base.OnDeactivate();
        }

        public override void Dispose()
        {
            _insideFSM?.Dispose();
            _insideFSM = null;
            
            base.Dispose();
        }
    }
}