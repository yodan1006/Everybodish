using UnityEngine;

namespace Grab.Runtime
{
    public class Grabable : MonoBehaviour, IGrabable
    {
        private Grabber grabber = null;

        public bool IsGrabbed()
        {
            return grabber != null;
        }

        public void Release()
        {
            grabber = null;
        }

        public bool TryGrab(Grabber newGrabber)
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
