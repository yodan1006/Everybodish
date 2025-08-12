using UnityEngine;
namespace Grab.Runtime
{
    public class RigidbodyGrabber : Grabber, IRigidbodyGrabber
    {

        protected Rigidbody heldRigidbody;
        protected float tempDamping;
        [SerializeField] protected Transform targetPosition;

        [Header("Physics Parameters")]

        [SerializeField] protected float pickupForce = 1f;
        [SerializeField] protected float heldLinearDamping = 10f;

        [Header("Area constraints settings")]
        public RigidbodyConstraints holdAreaConstraints;
        public RigidbodyConstraints releaseAreaConstraints;
        protected void Update()
        {
            if (heldRigidbody != null)
            {
                MoveObject();
            }
        }
        private void PickupRb(Rigidbody rb)
        {
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
            if (gameObject.TryGetComponent<Grabable>(out Grabable grabable) && gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Debug.Log("Grabable Rigidbody found", this);
                if (TryGrab(grabable))
                {
                    PickupRb(rb);
                    successfulGrab = true;
                }
            }
            return successfulGrab;
        }

        private new bool TryGrab(Grabable newGrabable)
        {
            return base.TryGrab(newGrabable);
        }

        public new void Release()
        {
            DropObject();
            base.Release();
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
            if (Application.isPlaying)
            {
                // Draw a sphere where the OverlapSphere is (positioned where your GameObject is as well as a size)
                Gizmos.DrawSphere(targetPosition.position, 0.05f);
            }
        }
    }
}