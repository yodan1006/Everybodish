using UnityEngine;

public interface IRigidbodyGrabber : IGrabber
{
    bool TryGrab(GameObject gameObject);
}
