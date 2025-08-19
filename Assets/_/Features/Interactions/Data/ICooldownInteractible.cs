using UnityEngine.Events;

namespace Interactions.Data
{
    public interface ICooldownInteractible : IInteractable
    {
        UnityEvent OnCooldownStart { get; }
        UnityEvent OnCooldownEnd { get; }
        new void Release();
        new bool TryInteract(IInteractor newGrabber);
    }
}
