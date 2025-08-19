using Interactions.Data;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    public class TimedInteractible : CooldownInteractible, ITimedInteractible
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

        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}
