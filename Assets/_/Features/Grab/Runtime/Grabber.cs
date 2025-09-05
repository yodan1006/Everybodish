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

        public virtual bool TryGrab(IGrabable newGrabable)
        {
            Log("Grabber.TryGrab");
            bool success = false;
            if (newGrabable.gameObject != transform.gameObject)
            {
                Vector3 grabberPosition = transform.position;
                Vector3 grabablePosition = newGrabable.transform.position;
                float distance = Vector3.Distance(grabberPosition, grabablePosition);
                if (distance < maxGrabRange)
                {
                    if (!IsGrabbing())
                    {
                        if (newGrabable.IsGrabable)
                        {
                            if (newGrabable.TryGrab(this))
                            {
                                grabable = newGrabable;
                                success = true;
                                Log("Grab successful", this);
                            }
                            else
                            {
                                Log("Mysterious forces oppose themselves to your grabbing this object.", this);
                            }

                        }
                        else
                        {
                            Log("Grabber is not currently grabable.  Grab failed.", this);
                        }

                    }
                    else
                    {
                        Log("Grabber is already grabbing something. Grab failed.", this);
                    }
                }
                else
                {
                    Log($"Grabbed item was over grab range. Is something wrong? Distance:{distance} Grab Range:{maxGrabRange}", this);
                }
            }
            else
            {
                Log("STOP GRABBING YOURSELF.", this);
            }
            return success;
        }

        public virtual bool Release()
        {
            bool success = false;
            if (IsGrabbing())
            {
                success = grabable.Release();
                grabable = null;
            }
            return success;
        }

        public virtual bool IsGrabbing()
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