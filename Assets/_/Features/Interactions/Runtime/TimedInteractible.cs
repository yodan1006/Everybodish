using Interactions.Data;
using UnityEngine.Events;

namespace Interactions.Runtime
{
    public class TimedInteractible : CooldownInteractible, ITimedInteractible
    {
        public UnityEvent onInteractionStart => throw new System.NotImplementedException();

        public UnityEvent onInteractionEnd => throw new System.NotImplementedException();

        public UnityEvent onInteractionCanceled => throw new System.NotImplementedException();

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
