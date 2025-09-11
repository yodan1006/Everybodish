using System.Linq;
using Round.Runtime;
using Spawner.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InteractionPlayerStart.Runtime
{
    public class activateScriptOnSwitchScene : MonoBehaviour
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
                var allPlayers = FindObjectsOfType<PlayerInput>().ToList();

                var round = FindFirstObjectByType<RoundSystem>();
                round.playerList = allPlayers;
            }
            
        }
        }
    }

