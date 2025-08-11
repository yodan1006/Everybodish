using Toolbox.Rigidbody.Runtime;
using UnityEngine;

public class RigidbodyGrabber : Grabber, IRigidbodyGrabber
{

    private Rigidbody heldRigidbody;
    private float tempDamping;
    protected Transform targetPosition;

    [Header("Physics Parameters")]
    [SerializeField] public float holdRange = 2.0f;
    [SerializeField] public float pickupRange = 5.0f;
    [SerializeField] public float pickupForce = 1f;
    [SerializeField] public float heldLinearDamping = 10f;

    [Header("Area constraints settings")]
    public RigidbodyConstraints holdAreaConstraints;
    public RigidbodyConstraints releaseAreaConstraints;
    protected void PickupObject(GameObject pickedObject)
    {
        Rigidbody rb = pickedObject.GetComponentInChildren<Rigidbody>();
        if (rb != null)
        {

            heldRigidbody = rb;
            rb.useGravity = false;
            tempDamping = heldRigidbody.linearDamping;
            heldRigidbody.linearDamping = heldLinearDamping;
            heldRigidbody.constraints = holdAreaConstraints;
        }
    }

    protected void DropObject()
    {
        heldRigidbody.useGravity = true;
        heldRigidbody.linearDamping = tempDamping;
        heldRigidbody.constraints = releaseAreaConstraints;

        heldRigidbody.transform.parent = null;
    }

    protected void MoveObject()
    {
        if (Vector3.Distance(heldRigidbody.transform.position, targetPosition.position) > 0.1f)
        {
            Vector3 moveDirection = (targetPosition.position - heldRigidbody.transform.position);
            heldRigidbody.AddForce(moveDirection * pickupForce);
        }
    }

    public bool TryGrab(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    private new bool TryGrab(Grabable newGrabable)
    {
      return base.TryGrab(newGrabable);
    }
}
