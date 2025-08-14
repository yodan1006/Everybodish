using DebugBehaviour.Runtime;
using Interactions.Data;

namespace Interactions.Runtime
{
    public class Interactable : VerboseMonoBehaviour, IInteractable
    {
        protected IInteractor interactor;
        public bool IsInteracted()
        {
            return interactor != null;
        }

        public void Release()
        {
            interactor = null;
        }

        public bool TryInteract(IInteractor newInteractor)
        {
            bool success = false;
            if (interactor == null)
            {
                interactor = newInteractor;
            }
            return success;
        }

        IInteractor IInteractable.Interactor()
        {
            return interactor;
        }
    }
}
