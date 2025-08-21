using UnityEngine;

namespace Interactions.Data
{
    public interface IInteractable
    {
        string name { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        bool CanInteract { get; }
        IInteractor Interactor { get; }
        void Release();
        bool TryInteract(IInteractor newGrabber);
        bool IsInteracted();
        void StartInteraction();
    }
}
