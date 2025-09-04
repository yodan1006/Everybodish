using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TransitionScene.Runtime
{
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance;
        [SerializeField] private float fakeLoadingDuration = 3f; // Durée de l'écran de chargement

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadSceneWithLoading(int targetScene)
        {
            StartCoroutine(LoadSceneProcess(targetScene));
        }

        private IEnumerator LoadSceneProcess(int targetScene)
        {
            yield return StartCoroutine(TransitManager.Instance.FadeOut());

            // 2. Sécurité : forcer le noir complet (au cas où un frame traîne)
            TransitManager.Instance.SetBlack();

            // 3. Charger l’écran de chargement
            SceneManager.LoadScene("LoadingScene");
            yield return null; // attendre 1 frame → éviter le flash

            // 4. Attendre le temps défini (fake loading)
            yield return new WaitForSeconds(fakeLoadingDuration);

            // 5. Charger la vraie scène
            SceneManager.LoadScene(targetScene);
            yield return null; // attendre 1 frame

            // 6. FadeIn → retour au jeu
            yield return StartCoroutine(TransitManager.Instance.FadeIn());
        }
    }
}
