using System.Collections.Generic;
using System.Linq;
using Round.Runtime;
using Skins.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TransitionScene.Runtime
{
    public class GameCleanup : MonoBehaviour
    {
        private void OnDestroy()
        {
            List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None).ToList();
            List<RoundSystem> roundSystems = FindObjectsByType<RoundSystem>(FindObjectsSortMode.None).ToList();
            List<LobbyManager> lobbyManagers = FindObjectsByType<LobbyManager>(FindObjectsSortMode.None).ToList();

            //Hasta la vista baby

            for (int i = 0; i < playerInputs.Count; i++)
            {
                playerInputs[i].DeactivateInput();
                Destroy(playerInputs[i].gameObject);
            }

            for (int i = 0; i < roundSystems.Count; i++)
            {
                Destroy(roundSystems[i].gameObject);
            }

            for (int i = 0; i < lobbyManagers.Count; i++)
            {
                Destroy(lobbyManagers[i].gameObject);
            }
        }
    }
}
