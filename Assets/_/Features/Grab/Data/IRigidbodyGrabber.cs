using Grab.Data;
using static UnityEngine.InputSystem.InputAction;
namespace Grab.Runtime
{
    public interface IRigidbodyGrabber : IGrabber
    {
        new void OnGrabAction(CallbackContext callbackContext);
        new void OnRelease(CallbackContext callbackContext);
    }
}