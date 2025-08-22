using DebugBehaviour.Runtime;
using Grab.Data;
using UnityEngine;

namespace Grab.Runtime
{
    public class Grabable : VerboseMonoBehaviour, IGrabable
    {
        private IGrabber grabber = null;

        [SerializeField] private RigidbodyConstraints holdAreaConstraints = RigidbodyConstraints.None;
        [SerializeField] private RigidbodyConstraints releaseAreaConstraints = RigidbodyConstraints.FreezeRotation;
        [SerializeField] private GrabableBehaviourEnum grabbedBehaviour = GrabableBehaviourEnum.None;
        [SerializeField] private MovementStrategyEnum movementStrategy = MovementStrategyEnum.Hold;
        [SerializeField] private Vector3 holdDistanceFromPlayer = new(0, 1, 1);
        [SerializeField] private bool isGrabable = true;

        public Vector3 HoldDistanceFromPlayerCenter { get => holdDistanceFromPlayer; }
        public RigidbodyConstraints ReleaseAreaConstraints { get => releaseAreaConstraints; }
        public MovementStrategyEnum MovementStrategy { get => movementStrategy; }
        public RigidbodyConstraints HoldAreaConstraints { get => holdAreaConstraints; }
        public GrabableBehaviourEnum GrabbedBehaviour { get => grabbedBehaviour; }
        bool IGrabable.IsGrabable { get => isGrabable; set => isGrabable = value; }

        public bool IsGrabbed()
        {
            return grabber != null;
        }

        public void Release()
        {
            grabber = null;
        }



        public bool TryGrab(IGrabber newGrabber)
        {
            bool success = false;
            if (isGrabable == true)
            {
                if (grabber == null)
                {
                    grabber = newGrabber;
                    success = true;
                }
                else if (newGrabber.gameObject == transform.gameObject)
                {
                    Debug.LogError("STOP GRABBING YOURSELF!", this);
                }
                else
                {
                    Debug.Log("Grab denied as someone is already holding it.");
                }
            }

            return success;
        }

        IGrabber IGrabable.Grabber()
        {
            return grabber;
        }
        public void SetColliderExcludeLayers(LayerMask excludeLayers)
        {
            if(TryGetComponent<Collider>(out Collider collider)){
                collider.excludeLayers = excludeLayers;
            }else
            {
                Debug.LogError("Grabable has no collider!");
            }
        }
    }
}
