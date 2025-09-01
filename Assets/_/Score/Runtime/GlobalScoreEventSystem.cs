using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Score.Runtime
{


    public class GlobalScoreEventSystem : MonoBehaviour
    {
        public static GlobalScoreEventSystem Instance { get; private set; }

        private readonly List<ScoreEvent> scoreEventLog = new();
        private readonly Dictionary<PlayerID, int> playerScores = new();

        public IReadOnlyList<ScoreEvent> ScoreEventLog => scoreEventLog.AsReadOnly();
        public IReadOnlyDictionary<PlayerID, int> PlayerScores => playerScores;

        [Header("Score Events")]
        public ScoreEventUnityEvent OnScoreEvent = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            foreach (PlayerID player in System.Enum.GetValues(typeof(PlayerID)))
            {
                playerScores[player] = 0;
            }
        }

        public void RegisterScoreEvent(PlayerID player, ScoreEventType eventType, int scoreDelta, PlayerID? targetPlayer = null)
        {
            var scoreEvent = new ScoreEvent(player, eventType, scoreDelta, targetPlayer);
            scoreEventLog.Add(scoreEvent);

            if (playerScores.ContainsKey(player))
                playerScores[player] += scoreDelta;
            else
                playerScores[player] = scoreDelta;

            Debug.Log($"[ScoreEvent] {player} - {eventType} - Score: {scoreDelta}" +
                      (targetPlayer.HasValue ? $" (Target: {targetPlayer.Value})" : "") +
                      $" @ {scoreEvent.TimeStamp}");

            OnScoreEvent.Invoke(scoreEvent);
        }

        public int GetScore(PlayerID player)
        {
            return playerScores.TryGetValue(player, out int score) ? score : 0;
        }

        public List<(PlayerID player, int score)> GetLeaderboard()
        {
            return playerScores
                .OrderByDescending(pair => pair.Value)
                .Select(pair => (pair.Key, pair.Value))
                .ToList();
        }


        public void ResetAllScores()
        {
            scoreEventLog.Clear();
            foreach (PlayerID player in playerScores.Keys)
                playerScores[player] = 0;
        }
    }

}
