using DebugBehaviour.Runtime;
using Grab.Data;
using UnityEngine;
namespace Grab.Runtime
{
    public class Grabber : VerboseMonoBehaviour, IGrabber
    {
        private IGrabable grabable;
        [SerializeField] protected float maxGrabRange = 5.0f;

        protected IGrabable Grabable { get => grabable; }

        public bool TryGrab(IGrabable newGrabable)
        {
            Debug.Log("Grabber.TryGrab");
            bool success = false;
            if (newGrabable.gameObject != transform.gameObject)
            {
                Vector3 grabberPosition = transform.position;
                Vector3 grabablePosition = newGrabable.transform.position;
                float distance = Vector3.Distance(grabberPosition, grabablePosition);
                if (distance < maxGrabRange)
                {
                    if (newGrabable.TryGrab(this))
                    {
                        grabable = newGrabable;
                        success = true;
                    }
                    else
                    {
                        Debug.Log("Grab failed. Is something already holding the grabable?");
                    }
                }
                else
                {
                    Debug.LogError($"Grabbed item was over grab range. Is something wrong? Distance:{distance} Grab Range:{maxGrabRange}", this);
                }
            }
            else
            {
                Debug.LogError("STOP GRABBING YOURSELF.", this);
            }
            return success;
        }

        public void Release()
        {
            if (IsGrabbing())
            {
                grabable.Release();
                grabable = null;
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