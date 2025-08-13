using DebugBehaviour.Runtime;
using Interactions.Data;

namespace Interactions.Runtime
{
    public class Interactible : VerboseMonoBehaviour, IInteractible
    {
        public bool IsInteracted()
        {
            throw new System.NotImplementedException();
        }

        public void Release()
        {
            throw new System.NotImplementedException();
        }

        public bool TryInteract(IInteractor newGrabber)
        {
            throw new System.NotImplementedException();
        }

        IInteractor IInteractible.Interactor()
        {
            throw new System.NotImplementedException();
        }
    }
}
