using UnityEngine;
namespace Grab.Runtime
{
    public class RigidbodyGrabber : Grabber, IRigidbodyGrabber
    {

        protected Rigidbody heldRigidbody;
        protected float tempDamping;
        protected Transform targetPosition;

        [Header("Physics Parameters")]
        [SerializeField] protected float holdRange = 2.0f;
        [SerializeField] protected float pickupRange = 5.0f;
        [SerializeField] protected float pickupForce = 1f;
        [SerializeField] protected float heldLinearDamping = 10f;

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
            bool successfulGrab = false;
            if (TryGetComponent<Grabable>(out Grabable grabable) && TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                if (TryGrab(grabable))
                {
                    heldRigidbody = rb;
                    successfulGrab = true;
                }
            }
            return successfulGrab;
        }

        private new bool TryGrab(Grabable newGrabable)
        {
            return base.TryGrab(newGrabable);
        }
    }
}