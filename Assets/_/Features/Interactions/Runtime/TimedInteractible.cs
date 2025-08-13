using Interactions.Data;

namespace Interactions.Runtime
{
    public class TimedInteractible : CooldownInteractible, ITimedInteractible
    {
        public void OnInteractCanceled()
        {
            throw new System.NotImplementedException();
        }

        public void OnInteractEnd()
        {
            throw new System.NotImplementedException();
        }

        public void OnInteractStart()
        {
            throw new System.NotImplementedException();
        }

    }
}
