using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ICooldownInteractor : IInteractor
    {
        UnityEvent OnInteractionAvailable { get; }
        UnityEvent OnInteractionUnavailable { get; }
        new void Release();
        new bool TryInteract(IInteractor newGrabber);
    }
}
