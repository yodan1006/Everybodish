using DebugBehaviour.Runtime;
using Interactions.Data;

namespace Interactions.Runtime
{
    public class Interactor : VerboseMonoBehaviour, IInteractor
    {
        private IInteractable interactible;
        public bool IsInteracting()
        {
            return interactible != null;
        }

        public void StopInteraction()
        {
            interactible.Release();
            interactible = null;
        }

        public bool TryGrab(IInteractable newInteractible)
        {
            bool success = false;
            if (!IsInteracting() && !newInteractible.IsInteracted())
            {
                if (interactible.TryInteract(this))
                {
                    interactible = newInteractible;
                    success = true;
                }
            }
            return success;
        }
    }
}
