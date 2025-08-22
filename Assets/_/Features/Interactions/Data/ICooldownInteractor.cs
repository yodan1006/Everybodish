using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ICooldownInteractor : IInteractor
    {
        UnityEvent OnInteractionAvailable { get; }
        UnityEvent OnInteractionUnavailable { get; }
        void Release();
        bool TryInteract(IInteractor newGrabber);
    }
}
