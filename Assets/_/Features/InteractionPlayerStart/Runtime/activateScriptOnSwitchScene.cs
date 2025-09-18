using System.Collections.Generic;
using System.Linq;
using Round.Runtime;
using Spawner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
{
    public class ActivateScriptOnSwitchScene : MonoBehaviour
    {
        public SpawnPoint spawner;

        private void Awake()
        {
            List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();
            if (RoundSystem.Instance != null)
            {
                foreach (PlayerInput playerInput in playerInputs)
                {
                    RoundSystem.Instance.JoinRound(playerInput);
                    spawner.OnPlayerSpawned(playerInput);
                }
            }
        }
    }
}

