using System.Collections.Generic;
using UnityEngine;
namespace Grab.Runtime
{
    public class TriggerAreaGrabber : RigidbodyGrabber, IProximityGrabber
    {
        [SerializeField] protected Vector3 grabAreaCenter;
        [SerializeField] protected float grabAreaRadius;
        [SerializeField] protected LayerMask layerMask;
        public Collider[] GetCollidersInArea()
        {
            // Use the OverlapBox to detect if there are any other colliders within this box area.
            // Use the GameObject's center, half the size (as a radius), and rotation. This creates an invisible box around your GameObject.
            return Physics.OverlapSphere(grabAreaCenter, grabAreaRadius, layerMask);
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

        public bool TryGrabClosestAvailable(Grabable[] grabables)
        {
            Grabable closestAvailableGrabable = null;
            int i = 0;
            do
            {
                if (closestAvailableGrabable != null)
                {
                    if (!grabables[i].IsGrabbed())
                    {

                    }

                }
                else
                {
                    closestAvailableGrabable = grabables[i];
                }
            } while (i < grabables.Length);
            return closestAvailableGrabable.TryGrab(this);
        }

        // Draw the Box Overlap as a gizmo to show where it currently is testing. Click the Gizmos button to see this.
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            // Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
            if (Application.isPlaying)
            {
                // Draw a sphere where the OverlapSphere is (positioned where your GameObject is as well as a size)
                Gizmos.DrawSphere(grabAreaCenter, grabAreaRadius);
            }
        }
    }
}