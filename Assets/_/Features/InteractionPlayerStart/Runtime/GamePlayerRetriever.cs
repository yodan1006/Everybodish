using Round.Runtime;
using Skins.Runtime;
using Spawner.Runtime;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputMapManager.Runtime
{
    public class GamePlayerRetriever : MonoBehaviour
    {
        public SpawnPoint spawner;

        private void Start()
        {
            // Get all PlayerInput objects and sort them by lobby slot index
            List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None)
                .OrderBy(p => p.GetComponentInChildren<SelectSkin>().GetSlotIndex())
                .ToList();
            // Debug.LogError($"Found {playerInputs.Count} players");
            if (RoundSystem.Instance != null)
            {
                foreach (PlayerInput playerInput in playerInputs)
                {
                    RoundSystem.Instance.JoinRound(playerInput);
                    spawner.OnPlayerSpawned(playerInput);
                }
            }
            else
            {
                Debug.LogError("ROUND IS NOT INITIALIZED YET");
            }
        }
    }
}

