using System;
using System.Collections.Generic;
using Interactions.Data;
using UnityEngine;

namespace Interactions.Runtime
{
    public class ProximityInteractor : TimedInteractor, IProximityInteractor
    {
        [SerializeField]
        protected Transform InteractAreaCenter;
        public float InteractAreaRadius;
        public LayerMask layerMask;

        private new void Update()
        {
            base.Update();
        }
        public Collider[] GetCollidersInArea()
        {
            // Use the OverlapBox to detect if there are any other colliders within this box area.
            // Use the GameObject's center, half the size (as a radius), and rotation. This creates an invisible box around your GameObject.
            return Physics.OverlapSphere(InteractAreaCenter.position, InteractAreaRadius, layerMask);
        }

        public List<IInteractable> GetInteractibles(Collider[] colliders)
        {
            List<IInteractable> Interactibles = new();
            foreach (Collider collider in colliders)
            {
                Log($"{collider.name}");
                //self check
                if (collider.gameObject != transform.gameObject)
                {
                    Interactable Interactible = collider.gameObject.GetComponentInChildren<Interactable>();
                    if (Interactible != null)
                    {
                        Interactibles.Add(Interactible);
                    }
                }
                else
                {
                    Log("Player collider is overlapping with Interact area. Filtering out.");
                }
            }
            return Interactibles;
        }

        public bool TryInteractClosestAvailable(List<IInteractable> Interactibles)
        {
            IInteractable closestAvailableInteractible = null;
            float closestInteractibleDistance = 0;
            int i = 0;
            bool success = false;
            if (Interactibles.Count > 0)
            {
                do
                {
                    Log($"{Interactibles[i].name}");
                    if (closestAvailableInteractible != null)
                    {
                        if (!Interactibles[i].IsInteracted())
                        {
                            if (Vector3.Distance(InteractAreaCenter.position, Interactibles[i].transform.position) < closestInteractibleDistance)
                            {
                                closestAvailableInteractible = Interactibles[i];
                                closestInteractibleDistance = Vector3.Distance(InteractAreaCenter.position, Interactibles[i].transform.position);
                            }
                        }
                    }
                    else
                    {
                        Log("Setting first Interactible by default");
                        closestAvailableInteractible = Interactibles[i];
                        closestInteractibleDistance = Vector3.Distance(InteractAreaCenter.position, Interactibles[i].transform.position);
                    }
                    i++;
                } while (i < Interactibles.Count);
                Log("AreaInteractber Interact attempt", this);
                success = TryInteract(closestAvailableInteractible.gameObject);
            }
            return success;
        }

        private bool TryInteract(GameObject gameObject)
        {
            throw new NotImplementedException();
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
                Gizmos.DrawWireSphere(InteractAreaCenter.position, InteractAreaRadius);
            }
        }

        public void OnInteractAction()
        {
            Log("Interact");
            Collider[] colliders = GetCollidersInArea();
            Log($"Found {colliders.Length} colliders", this);
            List<IInteractable> Interactibles = GetInteractibles(colliders);
            Log($"Found {Interactibles.Count} Interactibles", this);
            if (TryInteractClosestAvailable(Interactibles))
            {
                Log("Interact successful", this);
            }
            else
            {
                Log("Interact unsuccessful", this);
            }
        }
    }
}
