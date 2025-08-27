using System.Collections.Generic;
using Grab.Data;
using UnityEngine;
using UnityEngine.InputSystem;
namespace Grab.Runtime
{
    [DisallowMultipleComponent]
    public class ProximityGrabber : RigidbodyGrabber, IProximityGrabber
    {
        [SerializeField] protected Transform grabAreaCenter;
        public float grabAreaRadius = 1f;
        public LayerMask layerMask;


        public Collider[] GetCollidersInArea()
        {
            return Physics.OverlapSphere(grabAreaCenter.position, grabAreaRadius, layerMask);
        }

        public List<IGrabable> GetGrabables(Collider[] colliders)
        {
            List<IGrabable> grabables = new();
            foreach (Collider collider in colliders)
            {
                Log($"{collider.name}");
                //self check
                if (collider.gameObject != transform.gameObject)
                {
                    Grabable grabable = collider.gameObject.GetComponentInChildren<Grabable>();
                    if (grabable != null)
                    {
                        grabables.Add(grabable);
                    }
                }
                else
                {
                    Log("Player collider is overlapping with grab area. Filtering out.");
                }
            }
            return grabables;
        }

        public bool TryGrabClosestAvailable(List<IGrabable> grabables)
        {
            IGrabable closestAvailableGrabable = null;
            float closestGrabableDistance = 0;
            int i = 0;
            bool success = false;
            if (grabables.Count > 0)
            {
                do
                {
                    Log($"{grabables[i].name}");
                    if (closestAvailableGrabable != null)
                    {
                        if (!grabables[i].IsGrabbed() && grabables[i].IsGrabable)
                        {
                            if (Vector3.Distance(grabAreaCenter.position, grabables[i].transform.position) < closestGrabableDistance)
                            {
                                closestAvailableGrabable = grabables[i];
                                closestGrabableDistance = Vector3.Distance(grabAreaCenter.position, grabables[i].transform.position);
                            }
                        }
                    }
                    else
                    {
                        Log("Setting first grabable by default");
                        closestAvailableGrabable = grabables[i];
                        closestGrabableDistance = Vector3.Distance(grabAreaCenter.position, grabables[i].transform.position);
                    }
                    i++;
                } while (i < grabables.Count);
                Log("AreaGrabber grab attempt", this);
                success = TryGrab(closestAvailableGrabable);
            }
            return success;
        }

        private new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            if (Application.isPlaying)
            {
                Gizmos.DrawWireSphere(grabAreaCenter.position, grabAreaRadius);
            }
        }


        public new void OnRelease(InputAction.CallbackContext callbackContext)
        {
            if (IsGrabbing())
            {
                base.OnRelease(callbackContext);
            }
        }

        public override void OnGrabAction(InputAction.CallbackContext callbackContext)
        {
            if (!IsGrabbing())
            {
                Log("Grab");
                Collider[] colliders = GetCollidersInArea();
                Log($"Found {colliders.Length} colliders", this);
                List<IGrabable> grabables = GetGrabables(colliders);
                Log($"Found {grabables.Count} grabables", this);
                TryGrabClosestAvailable(grabables);
            }
        }

        void IProximityGrabber.OnGrabAction(InputAction.CallbackContext callbackContext)
        {
            OnGrabAction(callbackContext);
        }

        void IProximityGrabber.OnRelease(InputAction.CallbackContext callbackContext)
        {
            OnRelease(callbackContext);
        }
    }
}