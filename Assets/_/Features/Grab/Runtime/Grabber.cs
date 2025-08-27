using DebugBehaviour.Runtime;
using Grab.Data;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Grab.Runtime
{
    public abstract class Grabber : VerboseMonoBehaviour, IGrabber
    {
        private IGrabable grabable;
        [SerializeField] protected float maxGrabRange = 5.0f;

       public IGrabable Grabable { get => grabable; }

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
                    if (!IsGrabbing() && newGrabable.TryGrab(this))
                    {
                        grabable = newGrabable;
                        success = true;
                    }
                    else
                    {
                        Debug.Log("Grab failed. Is something already holding the grabable or is is not available?");
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

        public bool Release()
        {
            bool success = false;
            if (IsGrabbing())
            {
                success = grabable.Release();
                grabable = null;
            }
            return success;
        }

        public bool IsGrabbing()
        {
            return grabable != null;
        }

        private void OnDisable()
        {
            Release();
        }

        public abstract void OnGrabAction(InputAction.CallbackContext callbackContext);

        public abstract void OnRelease(InputAction.CallbackContext callbackContext);
    }
}