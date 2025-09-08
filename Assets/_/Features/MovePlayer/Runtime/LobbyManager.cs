using System.Collections.Generic;
using UnityEngine;

namespace MovePlayer.Runtime._.Features.MovePlayer.Runtime
{
    public class LobbyManager : MonoBehaviour
    {
        public static LobbyManager Instance { get; private set; }

        private readonly List<SelectSkin> players = new();

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
                players.Add(player);
        }

        public void UnregisterPlayer(SelectSkin player)
        {
            if (players.Contains(player))
                players.Remove(player);
        }

        public void CheckAllReady()
        {
            if (players.Count == 0) return;

            foreach (var player in players)
            {
                if (!player.IsReady)
                    return; // au moins un joueur pas prêt
            }

            // 🚨 Tous les joueurs sont prêts → on change de scène
            //SceneLoader.LoadSceneWithLoading(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}