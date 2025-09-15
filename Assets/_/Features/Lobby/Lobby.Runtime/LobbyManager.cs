using System.Collections.Generic;
using TransitionScene.Runtime;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Skins.Runtime
{
    public class LobbyManager : MonoBehaviour
    {
        public static LobbyManager Instance { get; private set; }

        private readonly List<SelectSkin> players = new();
        
        private SelectSkin[] playerSlots;


        [SerializeField] private GameObject[] UIjoin;
        public GameObject[] UIreeady;
        public GameObject[] UIValidate;
        public GameObject[] UIA;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            playerSlots = new SelectSkin[UIjoin.Length];
        }

        public void RegisterPlayer(SelectSkin player)
        {
            for (int i = 0; i < playerSlots.Length; i++)
            {
                if (playerSlots[i] == null)
                {
                    playerSlots[i] = player;
                    player.AssignSlotIndex(i);
                    UIjoin[i].SetActive(false);
                    UIreeady[i].SetActive(true);
                    break;
                }
            }

        }

        public void UnregisterPlayer(SelectSkin player)
        {
            int index = player.GetSlotIndex();
            if (index >= 0 && index < playerSlots.Length && playerSlots[index] == player)
            {
                playerSlots[index] = null;
                UIjoin[index].SetActive(true);
                UIreeady[index].SetActive(false);
                UIValidate[index].SetActive(false);
                UIA[index].SetActive(true);
            }
        }
        
        public int GetPlayerIndex(SelectSkin player)
        {
            return players.IndexOf(player);
        }
        
        public void CheckAllReady()
        {
            int connectedPlayers = 0;
            for (int i = 0; i < playerSlots.Length; i++)
            {
                var player = playerSlots[i];
                if (player != null)
                {
                    connectedPlayers++;
                    Debug.Log($"{player.name} Ready = {player.IsReady}");
                    if (!player.IsReady)
                        return; // Au moins un joueur pas prêt
                }
            }

            Debug.Log($"Nb joueurs enregistrés: {connectedPlayers}");
            if (connectedPlayers == 0) return;

            // 🚨 Tous les joueurs sont prêts → on change de scène
            var loader = FindFirstObjectByType<SceneLoader>();
            if (loader == null)
            {
                Debug.LogError("❌ SceneLoader introuvable dans la scène !");
                return;
            }
            loader.LoadSceneWithLoading(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }

    }
}