using UnityEngine;

namespace Machine.Runtime
{
    public class MultiCookStationTrigger : MonoBehaviour
    {
        [SerializeField] private UIMultiCook uiMultiCook;
        [SerializeField] private GameObject ToucheImage;

        private bool isPlayerInTrigger = false;

        private void Update()
        {
            // Ne fait le test que quand un joueur est dans le trigger
            if (isPlayerInTrigger)
            {
                if (gameObject.GetComponent<CookStationMultiIngredient>().IsCooking)
                    ToucheImage.SetActive(false);
                else
                    ToucheImage.SetActive(true);
            }
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