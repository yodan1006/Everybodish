using System.Collections.Generic;
using System.Linq;
using Round.Runtime;
using Spawner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
{
    public class LobbyPlayerRetrieverForGame : MonoBehaviour
    {
        public SpawnPoint spawner;

        private void Start()
        {
            Debug.LogError("Retrieveing players!", this);
            List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();
            Debug.LogError($"Found {playerInputs.Count} players");
            if (RoundSystem.Instance != null)
            {
                foreach (PlayerInput playerInput in playerInputs)
                {
                    RoundSystem.Instance.JoinRound(playerInput);
                    spawner.OnPlayerSpawned(playerInput);
                }
            } else
            {
                Debug.LogError("ROUND IS NOT INITIALIZED YET");
            }
        }
    }
}

