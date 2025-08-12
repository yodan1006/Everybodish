namespace Grab.Runtime
{
    internal interface IGrabable
    {
        protected IGrabber Grabber();
        void Release();
        bool TryGrab(Grabber newGrabber);
        bool IsGrabbed();
    }
}