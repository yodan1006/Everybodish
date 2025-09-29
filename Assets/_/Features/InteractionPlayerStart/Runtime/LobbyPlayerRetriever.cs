using System.Collections.Generic;
using System.Linq;
using Skins.Runtime;
using Spawner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
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
                    Debug.Log("Readding old player to lobby");
                    //TODO: Fix this
                    LobbyManager.Instance.RegisterPlayer(playerInput.GetComponentInChildren<SelectSkin>());
                    LobbyManager.Instance.UnregisterPlayer(playerInput.GetComponentInChildren<SelectSkin>());
                    spawner.OnPlayerSpawned(playerInput);
                }
            }
            else
            {
             //   Debug.LogError("LOBBY IS NOT INITIALIZED YET");
            }
        }
    }
}
