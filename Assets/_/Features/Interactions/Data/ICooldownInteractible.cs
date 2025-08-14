using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ICooldownInteractible : IInteractable
    {
        UnityEvent onCooldownStart { get; }
        UnityEvent onCooldownEnd { get; }
        new void Release();
        new bool TryInteract(IInteractor newGrabber);
    }
}
