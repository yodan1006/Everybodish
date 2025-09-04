using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TransitionScene.Runtime
{
    public class TransitManager : MonoBehaviour
    {
        public static TransitManager Instance;
        [SerializeField] private Image fadeImage;
        [SerializeField] private float fadeDuration = 1f;

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

        public IEnumerator FadeOut()
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                c.a = Mathf.Lerp(0, 1, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
            c.a = 1;
            fadeImage.color = c;
            fadeImage.gameObject.SetActive(false);
        }

        public IEnumerator FadeIn()
        {
            fadeImage.gameObject.SetActive(true);
            Color c = fadeImage.color;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                c.a = Mathf.Lerp(1, 0, t / fadeDuration);
                fadeImage.color = c;
                yield return null;
            }
            c.a = 0;
            fadeImage.color = c;
            // fadeImage.gameObject.SetActive(false);
        }
        public void SetBlack()
        {
            if (fadeImage != null)
            {
                Color c = fadeImage.color;
                c.a = 1; // opacité max
                fadeImage.color = c;
                fadeImage.gameObject.SetActive(true);
            }
        }
    }
}
