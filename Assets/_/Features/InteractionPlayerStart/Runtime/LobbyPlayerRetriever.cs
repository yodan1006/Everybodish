using System.Collections.Generic;
using System.Linq;
using Skins.Runtime;
using Spawner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputMapManager.Runtime
{
    public class LobbyPlayerRetriever : MonoBehaviour
    {
        public SpawnPoint spawner;

        private void OnEnable()
        {
            // Get all PlayerInput objects and sort them by lobby slot index
            List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None)
                .OrderBy(p => p.GetComponentInChildren<SelectSkin>().GetSlotIndex())
                .ToList();
            // Debug.LogError($"Found {playerInputs.Count} players");
            if (LobbyManager.Instance != null)
            {
                foreach (PlayerInput playerInput in playerInputs)
                {
                    LobbyManager.Instance.UnregisterPlayer(playerInput.GetComponentInChildren<SelectSkin>());
                    Destroy(playerInput.gameObject);
                }
            }
            else
            {
                //   Debug.LogError("LOBBY IS NOT INITIALIZED YET");
            }
        }
    }
}
