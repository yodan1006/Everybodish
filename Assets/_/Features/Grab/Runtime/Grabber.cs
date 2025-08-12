using UnityEngine;
namespace Grab.Runtime
{
    public class Grabber : MonoBehaviour, IGrabber
    {
        private Grabable grabable;

        public Grabable GetGrabable()
        {
            return grabable;
        }

        protected void SetGrabable(Grabable grabable)
        {
            this.grabable = grabable;
        }

        public bool TryGrab(Grabable newGrabable)
        {
            if (newGrabable.TryGrab(this))
            {
                SetGrabable(newGrabable);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Release()
        {
            if (IsGrabbing())
            {
                GetGrabable().Release();
            }
        }

        public bool IsGrabbing()
        {
            return grabable != null;
        }

        private void OnDisable()
        {
            Release();
        }
    }
}