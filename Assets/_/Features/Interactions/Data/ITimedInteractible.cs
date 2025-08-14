using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ITimedInteractible : ICooldownInteractible
    {
        UnityEvent onInteractionStart { get; }
        UnityEvent onInteractionEnd { get; }
        UnityEvent onInteractionCanceled { get; }
        new void Release();
        new bool TryInteract(IInteractor newGrabber);
        new void Update();

    }
}
