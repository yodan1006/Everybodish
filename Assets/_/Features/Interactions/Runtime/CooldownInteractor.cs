using Interactions.Data;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    public class CooldownInteractor : Interactor, ICooldownInteractor
    {
        public UnityEvent onInteractionAvailable = new();

        public UnityEvent onInteractionUnavailable = new();
        public UnityEvent OnInteractionAvailable { get => onInteractionAvailable; }

        public UnityEvent OnInteractionUnavailable { get => onInteractionUnavailable; }

        public void Release()
        {

        }

        public bool TryInteract(IInteractor newGrabber)
        {
            return false;
        }
    }
}
