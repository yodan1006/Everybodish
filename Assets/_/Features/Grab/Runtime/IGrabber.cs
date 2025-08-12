namespace Grab.Runtime
{
    public interface IGrabber
    {
        bool IsGrabbing();
        bool TryGrab(Grabable newGrabable);
        void Release();
    }
}