using System.Collections.Generic;
using System.Linq;
using Round.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InteractionPlayerStart.Runtime
{
    public class ActivateScriptOnSwitchScene : MonoBehaviour
    {


        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }


        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.buildIndex == 1)
            {
                List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();

                RoundSystem round = FindFirstObjectByType<RoundSystem>();
                if (round != null)
                {
                    foreach (PlayerInput playerInput in playerInputs)
                    {
                        round.JoinRound(playerInput);
                    }
                }

            }

        }
    }
}

