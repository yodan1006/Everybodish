using Skins.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InteractionPlayerStart.Runtime
{
    public class Control : MonoBehaviour
    {
        #region public
        [SerializeField] private string nameMenuControl;
        [SerializeField] private string nameMenuCredit;
        [SerializeField] private float quitterPressedTime = 0.5f;
        [SerializeField] private float longPressDuration = 1.0f;
        #endregion

        #region privé
        private PlayerInput playerInput;
        private LobbyManager lobbyManager;
        #endregion

        #region unity api

        /// <summary>
        /// Appelé au lancement du script, initialise les références à PlayerInput et LobbyManager.
        /// </summary>
        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();
            lobbyManager = FindFirstObjectByType<LobbyManager>();
        }
        #endregion

        #region main methode

        /// <summary>
        /// Affiche le menu de contrôle lorsque l'utilisateur effectue l'action liée.
        /// </summary>
        /// <param name="context">Contexte d'action d'entrée utilisateur.</param>
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

        /// <summary>
        /// Affiche le menu des crédits au déclenchement de l'entrée utilisateur spécifique.
        /// </summary>
        /// <param name="context">Contexte d'action d'entrée utilisateur.</param>
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

        /// <summary>
        /// Gère la fermeture des menus ou la sortie du joueur selon la durée d'appui sur l'entrée quitter.
        /// Pression longue : quitte l'application, pression courte : ferme les menus actifs ou retire le joueur.
        /// </summary>
        /// <param name="context">Contexte d'action d'entrée utilisateur.</param>
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
                        lobbyManager.UnregisterPlayer(gameObject.GetComponent<SelectSkin>());
                        // Si aucun menu actif → le joueur quitte la partie
                        Debug.Log($"Le joueur {playerInput.playerIndex} quitte la partie !");
                        Destroy(gameObject); // Supprime ce joueur
                    }
                }
            }
        }
        #endregion

        #region utils
        // Ajoutez ici toutes vos méthodes utilitaires privées
        #endregion
    }
}