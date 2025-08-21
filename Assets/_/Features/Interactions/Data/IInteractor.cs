using UnityEngine;
using UnityEngine.Events;

namespace Interactions.Data
{
    public interface IInteractor
    {
        UnityEvent OnInteractionSuccess { get; }
        UnityEvent OnInteractionFail { get; }
        GameObject gameObject { get; }
        IInteractable Interactable { get; }

        bool CanInteract();
        bool IsInteracting();
        bool TryInteract(IInteractable newInteractable);
        void StopInteraction();
    }
}
