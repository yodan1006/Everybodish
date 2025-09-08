using System;
using UnityEngine;

namespace Machine.Runtime
{
    public class MultiCookStationTrigger : MonoBehaviour
    {
        [SerializeField] private UIMultiCook uiMultiCook;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerInteract>(out var player))
            {
                uiMultiCook.ShowUI();
                player.CurrentMultiCookUI = uiMultiCook;
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent<PlayerInteract>(out var player))
            {
                uiMultiCook.HideUI();
                player.CurrentMultiCookUI = null;
            }
        }
    }
}