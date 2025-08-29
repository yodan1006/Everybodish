using UnityEngine;

namespace Grab.Data
{
    public interface IGrabable
    {
        string name { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        RigidbodyConstraints HoldAreaConstraints { get; }
        RigidbodyConstraints ReleaseAreaConstraints { get; }
        MovementStrategyEnum MovementStrategy { get; }
        GrabableBehaviourEnum GrabbedBehaviour { get; }
        Vector3 HoldDistanceFromPlayerCenter { get; }
        protected IGrabber Grabber();
        bool Release();
        bool TryGrab(IGrabber newGrabber);
        bool IsGrabbed();
        bool IsGrabable { get; }
        void SetColliderExcludeLayers(LayerMask excludeLayers);
    }
}