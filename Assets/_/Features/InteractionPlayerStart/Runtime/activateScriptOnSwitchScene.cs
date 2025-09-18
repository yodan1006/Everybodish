using System.Collections.Generic;
using System.Linq;
using Spawner.Runtime;
using Round.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
{
    public class ActivateScriptOnSwitchScene : MonoBehaviour
    {
       public  SpawnPoint spawner;

        private void Awake()
        {
            List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();

            RoundSystem round = FindFirstObjectByType<RoundSystem>();
            if (round != null)
            {
                foreach (PlayerInput playerInput in playerInputs)
                {
                    round.JoinRound(playerInput);
                    spawner.OnPlayerSpawned(playerInput);
                }
            }
        }
    }
}

