using System;
using DebugBehaviour.Runtime;
using Interactions.Data;

namespace Interactions.Runtime
{
    public class Interactor : VerboseMonoBehaviour, IInteractor
    {
        public bool IsInteracting()
        {
            throw new System.NotImplementedException();
        }

        public void StopInteraction()
        {
            throw new System.NotImplementedException();
        }

        public bool TryGrab(IInteractible newInteractible)
        {
            throw new System.NotImplementedException();
        }

        internal void OnDrawGizmos()
        {
            throw new NotImplementedException();
        }

        internal void Update()
        {
            throw new NotImplementedException();
        }
    }
}
