using System.Collections.Generic;
using System.Linq;
using ActionMap;
using Round.Runtime;
using Score.Runtime;
using Spawner.Runtime;
using TransitionScene.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Results.Runtime
{
    public class ResultScreenManager : MonoBehaviour
    {
        public SpawnPoint spawn;
        public PlayerResults playerResults;

        private readonly List<PlayerInputMap> inputMaps = new();

        #region Unity Api

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (RoundSystem.Instance != null)
            {
                //  Debug.LogError("Retrieveing players!", this);
                List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();
                //  Debug.LogError($"Found {playerInputs.Count} players");
                List<(int player, int score)> list = GlobalScoreEventSystem.GetLeaderboard();

                foreach ((int player, int score) in list)
                {
                    Debug.LogError($"Player {player} with {score} points");
                    //spawn players in leaderboard order
                    foreach (PlayerInput playerInput in playerInputs)
                    {
                        if (playerInput.playerIndex == player)
                        {
                            spawn.OnPlayerSpawned(playerInput);
                        }
                    }
                }
                foreach (PlayerInput playerInput in playerInputs)
                {
                    SpawnSystem spawnSystem = playerInput.GetComponent<SpawnSystem>();
                    //   spawnSystem.onPlayerQuit.AddListener(QuitScene);
                    PlayerInputMap inputMap = new()
                    {
                        devices = playerInput.devices
                    };
                    //For result screen
                    inputMap.Results.Restart.started += ctx => QuitScene(ctx);
                    inputMap.Results.Enable();
                    inputMaps.Add(inputMap);
                }
            }
            else
            {
                Debug.LogError("ROUND IS NOT INITIALIZED YET");
            }

        }

        private void QuitScene(InputAction.CallbackContext ctx)
        {
            FindFirstObjectByType<SceneLoader>().LoadSceneWithLoading(0);
            foreach (var item in inputMaps)
            {
                item.Results.Disable();
            }
        }
        #endregion

    }
}
