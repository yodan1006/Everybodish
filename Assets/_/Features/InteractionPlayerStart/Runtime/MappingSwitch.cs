using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InteractionPlayerStart.Runtime
{
    public class MappingSwitch : MonoBehaviour
    {
        private PlayerInput input;

        private void Start()
        {
            input = gameObject.GetComponent<PlayerInput>();
        }

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
            if (scene.buildIndex == 0)
            {
                input.SwitchCurrentActionMap("Lobby");
                var asset = input.actions;
                asset.FindActionMap("Player").Disable();
                
            }
            else
            {
                input.SwitchCurrentActionMap("Player");
                var asset = input.actions;
                asset.FindActionMap("Lobby").Disable();
            }
        }
    }
}
