using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ITimedInteractible : ICooldownInteractible
    {
        UnityEvent OnInteractionAvailable { get; }
        UnityEvent OnInteractionUnavailable { get; }
        new void Release();
        new bool TryInteract(IInteractor newGrabber);
    }
}
