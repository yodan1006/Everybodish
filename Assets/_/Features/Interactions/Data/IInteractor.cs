using UnityEngine;

namespace Interactions.Data
{
    public interface IInteractor
    {
        GameObject gameObject { get; }

        bool IsInteracting();
        bool TryGrab(IInteractable newInteractible);
        void StopInteraction();
    }
}
