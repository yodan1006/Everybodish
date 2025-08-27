using UnityEngine;
using UnityEngine.InputSystem;

namespace DebugBehaviour.Runtime
{
    public class PlayerInputManagerFinder : MonoBehaviour
    {
        private void Start()
        {
            var managers = FindObjectsByType<PlayerInputManager>(FindObjectsSortMode.None);
            Debug.Log("PlayerInputManager count: " + managers.Length);
            foreach (var m in managers)
            {
                Debug.Log("Found PlayerInputManager: " + m.name);
            }
        }
    }
}
