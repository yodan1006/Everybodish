using UnityEngine;
using UnityEngine.InputSystem;

namespace Machine.Runtime
{
    public class Playerinteract : MonoBehaviour
    {
        [SerializeField] private InputActionReference interactAction;
        [SerializeField] private PlayerInventory inventory;

        private CookStation stationInRange;

        private void OnEnable()
        {
            interactAction.action.performed += OnInteract;
        }

        private void OnDisable()
        {
            interactAction.action.performed -= OnInteract;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CookStation station))
            {
                stationInRange = station;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out CookStation station) && station == stationInRange)
            {
                stationInRange = null;
            }
        }

        private void OnInteract(InputAction.CallbackContext ctx)
        {
            if (stationInRange != null)
            {
                stationInRange.TryCook(inventory);
            }
        }
    }
}
