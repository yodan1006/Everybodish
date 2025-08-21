using DebugBehaviour.Runtime;
using Interactions.Data;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    public class Interactor : VerboseMonoBehaviour, IInteractor
    {
        private IInteractable interactable;
        public UnityEvent onInteractionSuccess = new();

        public UnityEvent onInteractionFail = new();

        public UnityEvent OnInteractionSuccess { get => onInteractionSuccess; }

        public UnityEvent OnInteractionFail { get => OnInteractionFail; }

        IInteractable IInteractor.Interactable => interactable;

        public bool IsInteracting()
        {
            return interactable != null;
        }


        public void StopInteraction()
        {
            interactable.Release();
            interactable = null;
        }

        public bool TryInteract(IInteractable newInteractable)
        {
            bool success = false;
            if (!IsInteracting() && !newInteractable.IsInteracted())
            {
                if (interactable.TryInteract(this))
                {
                    interactable = newInteractable;
                    success = true;
                }
            }
            return success;
        }

        public bool CanInteract()
        {
            return !IsInteracting();
        }
    }
}
