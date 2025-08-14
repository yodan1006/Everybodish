using UnityEngine;

namespace Interactions.Data
{
    public interface IInteractable
    {
        string name { get; }
        Transform transform { get; }
        GameObject gameObject { get; }

        protected IInteractor Interactor();
        void Release();
        bool TryInteract(IInteractor newGrabber);
        bool IsInteracted();
    }
}
