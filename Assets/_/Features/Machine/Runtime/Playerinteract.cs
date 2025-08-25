using UnityEngine;
using UnityEngine.InputSystem;

namespace Machine.Runtime
{
    [RequireComponent(typeof(PlayerInventory))]
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private Transform holdPoint;
        private GameObject heldObject;

        private void PickUp(GameObject obj)
        {
            if (heldObject != null) return;

            heldObject = obj;
            heldObject.transform.SetParent(holdPoint);
            heldObject.transform.localPosition = Vector3.zero;
        }

        private void Drop()
        {
            if (heldObject == null) return;

            heldObject.transform.SetParent(null);
            heldObject = null;
        }

        private void TryUseCookStation()
        {
            if (heldObject == null) return;

            var food = heldObject.GetComponent<Food>();
            if (food == null) return;

            // Détection station
            Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
            foreach (var hit in hits)
            {
                var station = hit.GetComponent<CookStation>();
                if (station != null)
                {
                    if (station.TryCook(food, out var resultPrefab))
                    {
                        heldObject = null; // l’ancien est détruit
                        station.SpawnCookedFood(resultPrefab);
                    }
                    return;
                }
            }
        }

        // ✅ Callbacks Input System
        public void OnInteract(InputAction.CallbackContext context)
        {
            if (!context.performed) return;

            if (heldObject == null)
            {
                // Essayer de ramasser un aliment
                Collider[] hits = Physics.OverlapSphere(transform.position, 2f);
                foreach (var hit in hits)
                {
                    var food = hit.GetComponent<Food>();
                    if (food != null)
                    {
                        PickUp(food.gameObject);
                        break;
                    }
                }
            }
            else
            {
                // Déposer
                Drop();
            }
        }

        public void OnUse(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TryUseCookStation();
            }
        }
    }
}
