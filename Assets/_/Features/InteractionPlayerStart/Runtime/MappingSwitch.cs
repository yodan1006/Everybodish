
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace InteractionPlayerStart.Runtime
{
    public class MappingSwitch : MonoBehaviour
    {
        /// <summary>
        /// MappingSwitch : Ce composant gère le changement dynamique des mappings d'actions (ActionMap) du joueur selon la scène chargée.
        /// Quand la scène de Lobby est chargée (indice 0), il applique le mapping "Lobby" ; sinon, il bascule sur "Player".
        /// Il s'assure aussi de désactiver les mappings inutilisés pour éviter les conflits d'inputs.
        /// </summary>
        #region privé
        private PlayerInput input;
        #endregion

        #region unity api

        private void Start()
        {
            input = gameObject.GetComponent<PlayerInput>();
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        /// <summary>
        /// Se désabonne de l'événement de chargement de scène lors de la désactivation du script.
        /// </summary>
        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion

        #region main methode

        /// <summary>
        /// Callback qui change l'ActionMap du joueur selon la scène chargée.
        /// Si la scène a l'index 0, on bascule sur "Lobby" ; sinon, sur "Player".
        /// Désactive en plus l'ActionMap opposée pour éviter les conflits.
        /// </summary>
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

        #endregion

        #region utils
        // Ajoutez ici vos méthodes utilitaires privées si besoin
        #endregion
    }
}