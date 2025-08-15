using Grab.Data;
using System.Collections.Generic;
using UnityEngine;
namespace Grab.Runtime
{
    public class ProximityGrabber : RigidbodyGrabber, IProximityGrabber
    {
        [SerializeField] protected Transform grabAreaCenter;
        public float grabAreaRadius;
        public LayerMask layerMask;

        private new void Update()
        {
            base.Update();
        }
        public Collider[] GetCollidersInArea()
        {
            // Use the OverlapBox to detect if there are any other colliders within this box area.
            // Use the GameObject's center, half the size (as a radius), and rotation. This creates an invisible box around your GameObject.
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
                        if (!grabables[i].IsGrabbed())
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

        // Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this.
        private new void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            Gizmos.color = Color.red;
            // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
            if (Application.isPlaying)
            {
                // Draw a sphere where the OverlapSphere is (positioned where your GameObject is as well as a size)
                Gizmos.DrawWireSphere(grabAreaCenter.position, grabAreaRadius);
            }
        }

        public void OnGrabAction()
        {
            Log("Grab");
            Collider[] colliders = GetCollidersInArea();
            Log($"Found {colliders.Length} colliders", this);
            List<IGrabable> grabables = GetGrabables(colliders);
            Log($"Found {grabables.Count} grabables", this);
            if (TryGrabClosestAvailable(grabables))
            {
                Log("Grab successful", this);
            }
            else
            {
                Log("Grab unsuccessful", this);
            }
        }

    }
}