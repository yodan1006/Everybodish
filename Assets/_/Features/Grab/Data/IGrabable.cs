using UnityEngine;

namespace Grab.Data
{
    public interface IGrabable
    {
        string name { get; }
        Transform transform { get; }
        GameObject gameObject { get; }
        RigidbodyConstraints releaseAreaConstraints { get; }

        protected IGrabber Grabber();
        void Release();
        bool TryGrab(IGrabber newGrabber);
        bool IsGrabbed();
    }
}