using UnityEngine;

namespace Grab.Runtime
{
    public class Grabable : MonoBehaviour, IGrabable
    {
        private IGrabber grabber = null;

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
            if (grabber == null)
            {
                grabber = newGrabber;
                return true;
            }
            else
            {
                return false;
            }
        }

        IGrabber IGrabable.Grabber()
        {
            return grabber;
        }
    }
}
