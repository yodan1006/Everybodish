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

        [SerializeField] private GameObject[] UIjoin;
        [SerializeField] private GameObject[] UIvalidateskins;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterPlayer(SelectSkin player)
        {
            if (!players.Contains(player))
            {
                players.Add(player);
                UIjoin[players.Count - 1].SetActive(false); // [ 0, 1, 2, 3]
                UIvalidateskins[players.Count - 1].SetActive(true); // [ 0, 1, 2, 3]
            }
        }

        public void UnregisterPlayer(SelectSkin player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
                UIjoin[players.Count].SetActive(true); // [ 0, 1, 2, 3]
                UIvalidateskins[players.Count].SetActive(false); // [ 0, 1, 2, 3]
            }
        }

        public void CheckAllReady()
        {
            Debug.Log($"Nb joueurs enregistrés: {players.Count}");
            if (players.Count == 0) return;

            foreach (var player in players)
            {
                Debug.Log($"{player.name} Ready = {player.IsReady}");
                if (!player.IsReady)
                    return; // au moins un joueur pas prêt
            }

            // 🚨 Tous les joueurs sont prêts → on change de scène
            //FindFirstObjectByType<SceneLoader>().LoadSceneWithLoading(SceneManager.GetActiveScene().buildIndex + 1);
            var loader = FindFirstObjectByType<SceneLoader>();
            if (loader == null)
            {
                Debug.LogError("❌ SceneLoader introuvable dans la scène !");
                return;
            }
            loader.LoadSceneWithLoading(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}