using UnityEngine;
using UnityEngine.SceneManagement;

namespace TransitionScene.Runtime
{
    public class SceneCharging : MonoBehaviour
    {
        public void SceneSuivante()
        {
            SceneLoader.Instance.LoadSceneWithLoading(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void SceneStart()
        {
            SceneLoader.Instance.LoadSceneWithLoading(0);
        }

        public void SceneEnd()
        {
            SceneLoader.Instance.LoadSceneWithLoading(3);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
