using System.Collections.Generic;
using UnityEngine;
namespace Grab.Runtime
{
    public class AreaGrabber : RigidbodyGrabber, IProximityGrabber
    {
        public Vector3 grabAreaCenter;
        public float grabAreaRadius;
        public LayerMask layerMask;
        public Collider[] GetCollidersInArea()
        {
            // Use the OverlapBox to detect if there are any other colliders within this box area.
            // Use the GameObject's center, half the size (as a radius), and rotation. This creates an invisible box around your GameObject.
            return Physics.OverlapSphere(transform.position + grabAreaCenter, grabAreaRadius, layerMask);
        }

        public static List<Grabable> GetGrabables(Collider[] colliders)
        {
            List<Grabable> grabables = new();
            foreach (Collider collider in colliders)
            {
                Grabable grabable = collider.gameObject.GetComponentInChildren<Grabable>();
                if (grabable != null)
                {
                    grabables.Add(grabable);
                }
            }
            return grabables;
        }

        public bool TryGrabClosestAvailable(List<Grabable> grabables)
        {
            Grabable closestAvailableGrabable = null;
            float closestGrabableDistance = 0;
            int i = 0;
            bool success = false;
            if (grabables.Count > 0)
            {
                do
                {
                    if (closestAvailableGrabable != null)
                    {
                        if (!grabables[i].IsGrabbed())
                        {
                            if (Vector3.Distance(transform.position + grabAreaCenter, grabables[i].transform.position) < closestGrabableDistance)
                            {
                                closestAvailableGrabable = grabables[i];
                                closestGrabableDistance = Vector3.Distance(transform.position + grabAreaCenter, grabables[i].transform.position);
                            }
                        }

                    }
                    else
                    {
                        closestAvailableGrabable = grabables[i];
                        closestGrabableDistance = Vector3.Distance(transform.position + grabAreaCenter, grabables[i].transform.position);
                    }
                } while (i < grabables.Count);
                success = closestAvailableGrabable.TryGrab(this);
            }
            return success;
        }

        // Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this.
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
            if (Application.isPlaying)
            {
                // Draw a sphere where the OverlapSphere is (positioned where your GameObject is as well as a size)
                Gizmos.DrawWireSphere(transform.position + grabAreaCenter, grabAreaRadius);
            }
        }

        public void OnGrabAction()
        {
            Debug.Log("Grab");
            if (TryGrabClosestAvailable(GetGrabables(GetCollidersInArea())))
            {
                Debug.Log("Grab successful");
            }
        }
    }
}