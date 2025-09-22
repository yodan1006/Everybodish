using System.Collections.Generic;
using TransitionScene.Runtime;
using UnityEngine;

namespace Skins.Runtime
{
    public class LobbyManager : MonoBehaviour
    {
        #region public

        public static LobbyManager Instance { get; private set; }
        public GameObject[] UiReady;
        public GameObject[] UiValidate;
        public GameObject[] UiAButton;

        #endregion

        #region unity api

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            //  DontDestroyOnLoad(gameObject);
            playerSlots = new SelectSkin[UiJoin.Length];
        }

        #endregion

        #region utils

        // Ajoutez ici toutes vos méthodes utilitaires privées si besoin

        #endregion

        #region main method

        /// <summary>
        /// Enregistre un joueur sur le premier slot disponible.
        /// Initialise l'UI correspondante (désactive "Join", active "Ready").
        /// </summary>
        public void RegisterPlayer(SelectSkin player)
        {
            for (int i = 0; i < playerSlots.Length; i++)
            {
                if (playerSlots[i] == null)
                {
                    playerSlots[i] = player;
                    player.AssignSlotIndex(i);
                    UiJoin[i].SetActive(false);
                    UiReady[i].SetActive(true);
                    break;
                }
            }
        }

        /// <summary>
        /// Désenregistre un joueur (slot libéré, UI réinitialisée).
        /// Active/désactive les bons éléments d'UI au retrait du joueur.
        /// </summary>
        public void UnregisterPlayer(SelectSkin player)
        {
            int index = player.GetSlotIndex();
            if (index >= 0 && index < playerSlots.Length && playerSlots[index] == player)
            {
                playerSlots[index] = null;
                UiJoin[index].SetActive(true);
                UiReady[index].SetActive(false);
                UiValidate[index].SetActive(false);
                UiAButton[index].SetActive(true);
            }
        }

        /// <summary>
        /// Retourne l'index du joueur dans la liste des joueurs enregistrés.
        /// Utile pour des actions spécifiques liées à un joueur.
        /// </summary>
        public int GetPlayerIndex(SelectSkin player)
        {
            return players.IndexOf(player);
        }

        /// <summary>
        /// Vérifie si tous les joueurs enregistrés sont prêts.
        /// Si oui, lance la scène suivante via le SceneLoader.
        /// </summary>
        public void CheckAllReady()
        {
            int connectedPlayers = 0;
            for (int i = 0; i < playerSlots.Length; i++)
            {
                var player = playerSlots[i];
                if (player != null)
                {
                    connectedPlayers++;
                    //Debug.Log($"{player.name} Ready = {player.IsReady}");
                    if (!player.IsReady)
                        return; // Au moins un joueur pas prêt
                }
            }

            //Debug.Log($"Nb joueurs enregistrés: {connectedPlayers}");
            if (connectedPlayers == 0) return;

            // 🚨 Tous les joueurs sont prêts → on change de scène
            var loader = FindFirstObjectByType<SceneLoader>();
            if (loader == null)
            {
                //Debug.LogError("❌ SceneLoader introuvable dans la scène !");
                return;
            }
            loader.LoadSceneWithLoading(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }

        #endregion

        #region private

        [SerializeField] private GameObject[] UiJoin;
        private readonly List<SelectSkin> players = new();
        private SelectSkin[] playerSlots;


        #endregion
    }
}