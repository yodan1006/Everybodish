using Toolbox.Rigidbody.Runtime;
using UnityEngine;

public class Grabber : MonoBehaviour, IGrabber
{
    public Grabable grabable;
    public bool TryGrab(Grabable newGrabable)
    {
        if (newGrabable.TryGrab(this))
        {
            grabable = newGrabable;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Release()
    {
        if (grabable != null)
        {
            grabable.Release();
        }
    }

    public bool IsGrabbing()
    {
        return grabable != null;
    }

    private void OnDisable()
    {
        Release();
    }
}
