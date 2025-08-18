using Grab.Data;
using UnityEngine;
namespace Grab.Runtime
{
    public class RigidbodyGrabber : Grabber, IRigidbodyGrabber
    {
        [Header("Physics Parameters")]
        [SerializeField] protected float pickupForce = 25f;
        [SerializeField] protected float heldLinearDamping = 10f;

        protected Rigidbody heldRigidbody;
        private float storedDamping;
        protected GameObject target;

        private void Awake()
        {
            target = new("Grabber target point");
            target.transform.parent = this.transform;
            target.SetActive(false);
        }

        protected void Update()
        {
            if (IsGrabbing())
            {
                ApplyMovementStrategy();
                ApplyHeldBehaviourStrategy();
            }
        }

        private void ApplyHeldBehaviourStrategy()
        {
            switch (Grabable.GrabbedBehaviour)
            {
                case GrabableBehaviourEnum.FaceGrabber:
                    AdjustObjectRotation();
                    break;
                default:
                    //do nothing
                    break;
            }
        }

        private void ApplyMovementStrategy()
        {
            switch (Grabable.MovementStrategy)
            {
                case MovementStrategyEnum.Hold:
                    //hold item in front of player and adjust rotation to face the player
                    MoveObject();
                    break;
                case MovementStrategyEnum.Drag:
                    //Player rotates around item, item must be dragged behind the player
                    MoveObject();
                    AdjustPlayerRotation();
                    break;
                default:
                    //do nothing
                    break;
            }
        }

        private void AdjustObjectRotation()
        {
            Quaternion targetRotation = Quaternion.Inverse(transform.rotation);
            Grabable.transform.rotation = targetRotation;

        }

        private void AdjustPlayerRotation()
        {
            Quaternion targetRotation = Quaternion.Inverse(Grabable.transform.rotation);
            transform.rotation = targetRotation;
        }

        private void PickupRbAndApplyConstraints(Rigidbody rb, RigidbodyConstraints constraints)
        {
            Debug.Log("Picked up Rigidbody", this);
            //store replaced variables
            storedDamping = rb.linearDamping;

            rb.useGravity = false;
            //replace stored variables
            rb.linearDamping = heldLinearDamping;
            rb.constraints = constraints;
            heldRigidbody = rb;

        }

        protected void DropObject(Rigidbody rb, RigidbodyConstraints constraints)
        {
            rb.useGravity = true;
            rb.linearDamping = storedDamping;
            rb.constraints = constraints;
            heldRigidbody = null;
        }

        protected void MoveObject()
        {
            Vector3 rBPosition = heldRigidbody.transform.position;
            Vector3 targetPosition = target.transform.position;
            if (Vector3.Distance(rBPosition, targetPosition) > 0.1f)
            {
                Vector3 moveDirection = targetPosition - rBPosition;
                heldRigidbody.AddForce(moveDirection * pickupForce);
            }
        }
        protected new bool TryGrab(IGrabable newGrabable)
        {
            bool successfulGrab = false;

            if (newGrabable.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Log("Rigidbody found", this);
                Log("Grabable component found", this);
                if (base.TryGrab(newGrabable))
                {
                    PickupRbAndApplyConstraints(rb, newGrabable.HoldAreaConstraints);
                    successfulGrab = true;
                    target.transform.position = transform.rotation * newGrabable.HoldDistanceFromPlayerCenter + transform.position;
                }
                else
                {
                    Log("Grab failed", this);
                }
            }
            else
            {
                Log("Rigidbody not found!", this);
            }
            return successfulGrab;
        }

        public new void Release()
        {
            if (IsGrabbing())
            {
                DropObject(heldRigidbody, Grabable.ReleaseAreaConstraints);
                base.Release();
            }
        }

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (Application.isPlaying)
            {
                if (IsGrabbing())
                {
                    Gizmos.DrawSphere(target.transform.position, 0.05f);
                }
            }
        }
    }
}