using UnityEngine;

namespace Interactions.Data
{
    public interface IInteractor
    {
        GameObject gameObject { get; }

        bool IsInteracting();
        bool TryGrab(IInteractible newInteractible);
        void StopInteraction();
    }
}
