using UnityEngine;

namespace Machine.Runtime
{
    public class MultiCookStationTrigger : MonoBehaviour
    {
        [SerializeField] private UIMultiCook uiMultiCook;
        [SerializeField] private GameObject ToucheImage;

        private bool isPlayerInTrigger = false;

        // Références éventuelles (une seule des deux sera non nulle)
        private CookStationMultiIngredient multiIngredient;
        private CookStation cookStation;

        private void Awake()
        {
            // On récupère les composants si présents
            multiIngredient = GetComponent<CookStationMultiIngredient>();
            cookStation = GetComponent<CookStation>();
        }

        private void Update()
        {
            if (!isPlayerInTrigger) return;

            bool isCooking = false;

            if (multiIngredient != null && multiIngredient.IsCooking)
                isCooking = true;

            if (cookStation != null && cookStation._isCooking)
                isCooking = true;

            // Si une des deux machines est en train de cuire, on cache l’image
            ToucheImage.SetActive(!isCooking);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerInteract>(out var player))
            {
                isPlayerInTrigger = true;
                uiMultiCook.ShowUI();
                player.CurrentMultiCookUI = uiMultiCook;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerInteract>(out var player))
            {
                isPlayerInTrigger = false;
                // On cache l’image quand le joueur quitte le trigger ?
                ToucheImage.SetActive(false);

                uiMultiCook.HideUI();
                player.CurrentMultiCookUI = null;
            }
        }
    }
}