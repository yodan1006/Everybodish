using Interactions.Data;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    public class TimedInteractable : CooldownInteractable, ITimedInteractible
    {
        public UnityEvent onInteractionAvailable = new();

        public UnityEvent onInteractionUnavailable = new();
        public UnityEvent OnInteractionAvailable { get; }
        public UnityEvent OnInteractionUnavailable { get; }

        public void OnInteractionCanceled()
        {

        }

        public void OnInteractionEnd()
        {
        }

        public void OnInteractionStart()
        {
        }

        public void Release()
        {
            throw new System.NotImplementedException();
        }

        public bool TryInteract(IInteractor newGrabber)
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
