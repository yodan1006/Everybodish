using Grab.Runtime;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
namespace Grab.Data
{
    public interface IProximityGrabber : IRigidbodyGrabber
    {
        Collider[] GetCollidersInArea();
        bool TryGrabClosestAvailable(List<IGrabable> grabables);
        new void OnGrabAction(CallbackContext callbackContext);
        new void OnRelease(CallbackContext callbackContext);
    }
}