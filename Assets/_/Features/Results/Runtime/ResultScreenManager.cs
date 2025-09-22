using System.Collections.Generic;
using System.Linq;
using Round.Runtime;
using Score.Runtime;
using Spawner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Results.Runtime
{
    public class ResultScreenManager : MonoBehaviour
    {
        #region Publics
        public SpawnPoint spawn;
        public PlayerResults playerResults;
        #endregion


        #region Unity Api

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            if (RoundSystem.Instance != null)
            {
                Debug.LogError("Retrieveing players!", this);
                List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();
                Debug.LogError($"Found {playerInputs.Count} players");
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
            }
            else
            {
                Debug.LogError("ROUND IS NOT INITIALIZED YET");
            }

        }
        #endregion

    }
}
