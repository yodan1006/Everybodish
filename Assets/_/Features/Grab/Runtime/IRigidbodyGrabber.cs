using UnityEngine;
namespace Grab.Runtime
{
    public interface IRigidbodyGrabber : IGrabber
    {
        bool TryGrab(GameObject gameObject);
    }
}