using DebugBehaviour.Runtime;
using UnityEngine;
namespace Grab.Runtime
{
    public class Grabber : VerboseMonoBehaviour, IGrabber
    {
        private Grabable grabable;
        [SerializeField] protected float maxGrabRange = 5.0f;
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
            bool result = false;
            if (newGrabable.gameObject != transform.gameObject)
            {
                if (Vector3.Distance(transform.position, newGrabable.transform.position) < maxGrabRange)
                {
                    if (newGrabable.TryGrab(this))
                    {
                        SetGrabable(newGrabable);
                        result = true;
                    }
                }
                else
                {
                    Debug.LogError("Grabbed item was over grab range. Is something wrong?", this);
                }
            }
            else
            {
                Debug.LogError("STOP GRABBING YOURSELF.", this);
            }
            return result;
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