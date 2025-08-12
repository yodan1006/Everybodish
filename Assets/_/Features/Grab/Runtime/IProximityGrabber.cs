using UnityEngine;
namespace Grab.Runtime
{
    public interface IProximityGrabber : IRigidbodyGrabber
    {
        Collider[] GetCollidersInArea();
        bool TryGrabClosestAvailable(Grabable[] grabables);
    }
}