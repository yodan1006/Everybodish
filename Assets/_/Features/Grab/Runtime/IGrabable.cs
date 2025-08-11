namespace Toolbox.Rigidbody.Runtime
{
    internal interface IGrabable
    {
        IGrabber Grabber();
        void Release();
        bool TryGrab(IGrabber newGrabber);
        bool IsGrabbed();
    }
}