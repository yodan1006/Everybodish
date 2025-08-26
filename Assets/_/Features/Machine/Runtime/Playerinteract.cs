using System;
using Grab.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Machine.Runtime
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInteract : MonoBehaviour
    {
       // [SerializeField] private Transform holdPoint;
       // private GameObject heldObject;
       AnimatedProximityGrabber grabber;
       [SerializeField] private float radiusDetector;

       // private void PickUp(GameObject obj)
        // {
        //     if (heldObject != null) return;
        //
        //     heldObject = obj;
        //     heldObject.transform.SetParent(holdPoint);
        //     heldObject.transform.localPosition = Vector3.zero;
        // }

        // private void Drop()
        // {
        //     if (heldObject == null) return;
        //
        //     heldObject.transform.SetParent(null);
        //     heldObject = null;
        // }
        private void Awake(){
            if (TryGetComponent<AnimatedProximityGrabber>(out grabber))
            {
                this.grabber = grabber;
            }
        }

        private void TryUseCookStation()
        {
            if (!grabber.IsGrabbing()) return;

            if (!grabber.Grabable.gameObject.TryGetComponent<Food>(out Food food) || food == null) 
                return;

            Collider[] hits = Physics.OverlapSphere(transform.position, 2f);

            foreach (var hit in hits)
            {
                // Cas 1 : CookStation simple
                if (hit.TryGetComponent<CookStation>(out var simpleStation))
                {
                    if (simpleStation.TryCook(food, out var resultPrefab))
                    {
                        grabber.Release();
                        //food = null;
                        //simpleStation.SpawnCookedFood(resultPrefab);
                    }
                    return;
                }

                // Cas 2 : CookStation multi
                if (hit.TryGetComponent<CookStationMultiIngredient>(out var multiStation))
                {
                    multiStation.AddFood(food);
                    grabber.Release();
                    //food = null;
                    return;
                }
            }
        }


        // ✅ Callbacks Input System
        // public void OnInteract(InputAction.CallbackContext context)
        // {
        //     if (!context.performed) return;
        //
        //     if (heldObject == null)
        //     {
        //         // Essayer de ramasser un aliment
        //         Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
        //         foreach (var hit in hits)
        //         {
        //             var food = hit.GetComponent<Food>();
        //             if (food != null)
        //             {
        //                 PickUp(food.gameObject);
        //                 break;
        //             }
        //         }
        //     }
        //     else
        //     {
        //         // Déposer
        //         Drop();
        //     }
        // }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryUseCookStation();
            }
        }
    }
}
