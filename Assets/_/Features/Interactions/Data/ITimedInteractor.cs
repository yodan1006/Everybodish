using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ITimedInteractor : ICooldownInteractor
    {
        UnityEvent OnInteractionStart { get; }
        UnityEvent OnInteractionEnd { get; }
        UnityEvent OnInteractionCanceled { get; }
        new void Release();
        new bool TryInteract(IInteractable newInteractable);
        bool TryCancelInteraction();
    }
}