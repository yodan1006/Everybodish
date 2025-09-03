using Grab.Data;
using PlayerLocomotion.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Grab.Runtime
{
    [RequireComponent(typeof(CameraRelativeRotation))]
    public abstract class RigidbodyGrabber : Grabber, IRigidbodyGrabber
    {
        [Header("Physics Parameters")]
        [SerializeField] protected ForceMode forceMode = ForceMode.VelocityChange;
        [SerializeField] protected float pickupForce = 25f;
        [SerializeField] protected float maxPickupForce = 50f;
        [SerializeField] protected float heldLinearDamping = 10f;
        [SerializeField] protected float snapbackDistanceMultiplier = 1.5f;
        [SerializeField] protected float damageVelocityThreshold = 1.0f;
        [SerializeField] protected LayerMask snapbackLayer;
        //TODO: item rotation over time
        //[SerializeField] protected float rotationSpeed = 10f;

        private Vector3 rBPosition;
        private Vector3 targetPosition;
        private Vector3 playerPosition;
        private float distanceToTarget;
        private float distanceToPlayer;
        private float desiredDistance;
        private float snapbackDistance;
        private Vector3 directionToTarget;
        private Vector3 directionToPlayer;
        private bool isObstructedFromTarget;
        private bool isObstructedFromPlayer;
        private bool shouldSnapBack;

        protected Rigidbody heldRigidbody;
        private float storedDamping;
        protected GameObject target;
        private CameraRelativeRotation rotationManager;

        protected void Awake()
        {
            target = new("Grabber target point");
            target.transform.parent = transform;
            target.SetActive(false);
            rotationManager = GetComponent<CameraRelativeRotation>();
        }

        protected void FixedUpdate()
        {
            if (IsGrabbing())
            {
                FixedUpdateHeldBehaviourStrategy();
            }
        }

        private void FixedUpdateHeldBehaviourStrategy()
        {
            switch (Grabable.GrabbedBehaviour)
            {
                case GrabableBehaviourEnum.FaceGrabber:
                    FixedUpdateMovementStrategy();
                    FixedUpdateRotateTowardsPlayer();
                    break;
                default:
                    FixedUpdateMovementStrategy();
                    break;
            }
        }

        private void FixedUpdateMovementStrategy()
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
                    break;
                default:
                    //do nothing
                    break;
            }
        }

        private void FixedUpdateRotateTowardsPlayer()
        {
            Quaternion targetRotation = Quaternion.LookRotation(-transform.forward);
            Grabable.transform.rotation = targetRotation;
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
            rBPosition = heldRigidbody.transform.position;
            targetPosition = target.transform.position;
            playerPosition = transform.position;

            distanceToTarget = Vector3.Distance(rBPosition, targetPosition);

            distanceToPlayer = Vector3.Distance(rBPosition, playerPosition);

            desiredDistance = Vector3.Distance(target.transform.localPosition, Vector3.zero);

            snapbackDistance = desiredDistance * snapbackDistanceMultiplier;

            // Raycast directions
            directionToTarget = (rBPosition - targetPosition).normalized;
            directionToPlayer = (rBPosition - playerPosition).normalized;

            isObstructedFromTarget = Physics.Raycast(targetPosition, directionToTarget, distanceToTarget, snapbackLayer);

            isObstructedFromPlayer = Physics.Raycast(playerPosition, directionToPlayer, distanceToPlayer, snapbackLayer);

            shouldSnapBack = distanceToPlayer > snapbackDistance && (isObstructedFromTarget || isObstructedFromPlayer ||
                                     distanceToTarget > snapbackDistance);

            if (shouldSnapBack == true)
            {
                // Move the object closer within snapbackDistance

                Vector3 teleportPosition = targetPosition;

                heldRigidbody.position = teleportPosition;
                heldRigidbody.linearVelocity = Vector3.zero;
                heldRigidbody.angularVelocity = Vector3.zero;

                LogWarning("Grabbable was too far from target or obstructed, snapping back closer");
            }
            else if (distanceToTarget > 0.1f && distanceToTarget < snapbackDistance)
            {
                Vector3 moveDirection = targetPosition - rBPosition;

                // Optionally clamp force for stability
                Vector3 clampedForce = Vector3.ClampMagnitude(moveDirection * pickupForce, maxPickupForce);

                heldRigidbody.AddForce(clampedForce, forceMode);
            }
        }

        protected void SetRotationStrategy(MovementStrategyEnum stategy)
        {
            switch (stategy)
            {
                case MovementStrategyEnum.Hold:
                    //hold item in front of player and adjust rotation to face the player
                    rotationManager.inverted = false;
                    break;
                case MovementStrategyEnum.Drag:
                    //Player rotates around item, item must be dragged behind the player
                    rotationManager.inverted = true;
                    break;
                default:
                    rotationManager.inverted = false;
                    break;
            }
        }

        public override bool TryGrab(IGrabable newGrabable)
        {
            bool successfulGrab = false;

            if (newGrabable.gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Log("Rigidbody found", this);
                if (base.TryGrab(newGrabable))
                {
                    Log($"Grabable component found : {rb.gameObject.name}", this);
                    int excludeLayers = 1 << LayerMask.NameToLayer("Player");
                    Grabable.SetColliderExcludeLayers(excludeLayers);
                    PickupRbAndApplyConstraints(rb, newGrabable.HoldAreaConstraints);
                    successfulGrab = true;
                    target.transform.position = transform.rotation * newGrabable.HoldDistanceFromPlayerCenter + transform.position;
                    SetRotationStrategy(newGrabable.MovementStrategy);
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

        protected void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (Application.isPlaying)
            {
                if (IsGrabbing())
                {
                    Gizmos.DrawSphere(targetPosition, 0.05f);
                    Gizmos.DrawSphere(playerPosition, 0.05f);
                    Gizmos.DrawSphere(rBPosition, 0.05f);

                    if (isObstructedFromPlayer == true)
                    {
                        DrawRay(playerPosition, directionToPlayer * distanceToPlayer, Color.red);
                    }
                    else
                    {
                        DrawRay(playerPosition, directionToPlayer * distanceToPlayer, Color.blue);
                    }

                    if (isObstructedFromTarget == true)
                    {
                        DrawRay(targetPosition, directionToTarget * distanceToTarget, Color.red);
                    }
                    else
                    {
                        DrawRay(targetPosition, directionToTarget * distanceToTarget, Color.blue);
                    }
                }
            }
        }

        public override bool Release()
        {
            bool success = false;
            if (IsGrabbing())
            {
                Grabable.SetColliderExcludeLayers(0);
                DropObject(heldRigidbody, Grabable.ReleaseAreaConstraints);
                success = base.Release();
                SetRotationStrategy(MovementStrategyEnum.None);
            }
            return success;
        }

        public override void OnRelease(InputAction.CallbackContext callbackContext)
        {
            Release();
        }

        void IRigidbodyGrabber.OnGrabAction(InputAction.CallbackContext callbackContext)
        {
            OnGrabAction(callbackContext);
        }

        void IRigidbodyGrabber.OnRelease(InputAction.CallbackContext callbackContext)
        {
            OnRelease(callbackContext);
        }
    }
}