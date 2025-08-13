using Grab.Data;
using UnityEngine;

namespace Grab.Runtime
{
    public class Grabable : MonoBehaviour, IGrabable
    {
        private IGrabber grabber = null;
        public RigidbodyConstraints holdAreaConstraints;
        public RigidbodyConstraints releaseAreaConstraints;
        public GrabableBehaviourEnum grabbedBehaviour;
        public MovementStrategyEnum movementStrategy;
        MovementStrategyEnum IGrabable.movementStrategy => movementStrategy;
        RigidbodyConstraints IGrabable.releaseAreaConstraints => releaseAreaConstraints;

        public bool IsGrabbed()
        {
            return grabber != null;
        }

        public void Release()
        {
            grabber = null;
        }

        public bool TryGrab(IGrabber newGrabber)
        {
            bool success = false;
            if (grabber == null)
            {
                grabber = newGrabber;
                success = true;
            }
            else if (newGrabber.gameObject == transform.gameObject)
            {
                Debug.LogError("STOP GRABBING YOURSELF!", this);
            }
            return success;
        }

        IGrabber IGrabable.Grabber()
        {
            return grabber;
        }
    }
}
