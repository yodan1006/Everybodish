using DebugBehaviour.Runtime;
using Interactions.Data;

namespace Interactions.Runtime
{
    public class Interactable : VerboseMonoBehaviour, IInteractable
    {
        protected bool canInteract = true;
        protected IInteractor interactor;

        public bool CanInteract { get => canInteract; set => canInteract = value; }

        public bool IsInteracted()
        {
            return interactor != null;
        }

        public void Release()
        {
            interactor = null;
        }

        public void StartInteraction()
        {
            Log(interactor.gameObject.name);
        }

        public bool TryInteract(IInteractor newInteractor)
        {
            bool success = false;
            if (interactor == null)
            {
                interactor = newInteractor;
                StartInteraction();
            }
            return success;
        }

        IInteractor IInteractable.Interactor()
        {
            return interactor;
        }
    }
}
