using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
{
    public class Control : MonoBehaviour
    {
        [SerializeField] private string nameMenuControl;
        [SerializeField] private string nameMenuCredit;
        [SerializeField] private float quitterPressedTime = 0.5f;
        
        [SerializeField] private float longPressDuration = 1.0f; // Durée pour considérer comme un appui long (en secondes)

        public void ControleMenu(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameObject uiManager = GameObject.Find("UIParentMenu");
                if (uiManager != null)
                {
                    Transform child = uiManager.transform.Find(nameMenuControl);
                    if (child != null)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning("Objet non trouvé : " + nameMenuControl);
                }
            }
        }

        public void CreditMenu(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                GameObject uiManager = GameObject.Find("UIParentMenu");
                if (uiManager != null)
                {
                    Transform child = uiManager.transform.Find(nameMenuCredit);
                    if (child != null)
                    {
                        child.gameObject.SetActive(true);
                    }
                }
                else
                {
                    Debug.LogWarning("Objet non trouvé : " + nameMenuCredit);
                }
            }
        }

        public void Quitter(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                // Début de l'appui
                quitterPressedTime = Time.time;
            }
            else if (context.canceled)
            {
                // Fin de l'appui
                float pressedDuration = Time.time - quitterPressedTime;
                if (pressedDuration >= longPressDuration)
                {
                    // Appui long : quitter l'application
                    Application.Quit();
                }
                else
                {
                    // Appui court : désactiver les menus si actifs
                    GameObject menuControl = GameObject.Find(nameMenuControl);
                    GameObject menuCredit = GameObject.Find(nameMenuCredit);

                    bool anyMenuActive = false;

                    if (menuControl != null && menuControl.activeSelf)
                    {
                        menuControl.SetActive(false);
                        anyMenuActive = true;
                    }
                    if (menuCredit != null && menuCredit.activeSelf)
                    {
                        menuCredit.SetActive(false);
                        anyMenuActive = true;
                    }

                    if (!anyMenuActive)
                    {
                        Debug.Log("Aucun menu actif à désactiver.");
                    }
                }
            }
        }
    }
}