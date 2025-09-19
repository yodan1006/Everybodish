using System.Collections.Generic;
using System.Linq;
using Round.Runtime;
using Spawner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Results.Runtime
{
    public class ResultScreenManager : MonoBehaviour
    {
        #region Publics
        public SpawnPoint spawn;
        #endregion


        #region Unity Api

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {

            //TODO : SORT PLAYERS BY SCORE
            if (RoundSystem.Instance != null)
            {
            //    List<PlayerInput> playerInputs = RoundSystem.Instance.Players();
             //   Debug.Log(playerInputs);
            }
            Debug.LogError("Retrieveing players!", this);
            List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();
            Debug.LogError($"Found {playerInputs.Count} players");
            if (RoundSystem.Instance != null)
            {
                foreach (PlayerInput playerInput in playerInputs)
                {
                    spawn.OnPlayerSpawned(playerInput);
                }
            }
            else
            {
                Debug.LogError("ROUND IS NOT INITIALIZED YET");
            }
        
        }

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion


        #region Main Methods
        public void OnPlayerJoined(PlayerInput input)
        {
            spawn.OnPlayerSpawned(input);
        }
        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
