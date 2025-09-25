using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace TransitionScene.Runtime
{
    public class TransitCredit : MonoBehaviour
    {
        public void Credit(InputAction.CallbackContext context)
        {
            SceneManager.LoadScene(4);
        }
    }
}
