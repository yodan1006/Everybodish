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
            if (RoundSystem.Instance != null)
            {
                System.Collections.Generic.List<PlayerInput> playerInputs = RoundSystem.Instance.Players();
                Debug.Log(playerInputs);
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
