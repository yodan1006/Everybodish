using UnityEngine;

namespace Grab.Data
{
    public interface IGrabber
    {
        GameObject gameObject { get; }

        bool IsGrabbing();
        bool TryGrab(IGrabable newGrabable);
        void Release();
    }
}