using static UnityEngine.InputSystem.InputAction;

namespace Grab.Data
{
    public interface IAnimatedProximityGrabber : IProximityGrabber
    {
        new void OnGrabAction(CallbackContext callbackContext);
        new void OnRelease(CallbackContext callbackContext);
    }
}
