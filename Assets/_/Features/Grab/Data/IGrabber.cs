using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace Grab.Data
{
    public interface IGrabber
    {
        GameObject gameObject { get; }

        bool IsGrabbing();
        bool TryGrab(IGrabable newGrabable);
        void OnGrabAction(CallbackContext callbackContext);
        void OnRelease(CallbackContext callbackContext);
    }
}