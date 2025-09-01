using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Score.Runtime
{


    public class GlobalScoreEventSystem : MonoBehaviour
    {
        public static GlobalScoreEventSystem Instance { get; private set; }

        private readonly List<ScoreEvent> scoreEventLog = new();
        private readonly Dictionary<int, int> playerScores = new();

        public IReadOnlyList<ScoreEvent> ScoreEventLog => scoreEventLog.AsReadOnly();
        public IReadOnlyDictionary<int, int> PlayerScores => playerScores;

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

            foreach (int player in playerScores.Keys)
            {
                playerScores[player] = 0;
            }
        }

        public void RegisterScoreEvent(int player, ScoreEventType eventType, int scoreDelta, int? targetPlayer = null)
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

        public int GetScore(int player)
        {
            return playerScores.TryGetValue(player, out int score) ? score : 0;
        }

        public List<(int player, int score)> GetLeaderboard()
        {
            return playerScores
                .OrderByDescending(pair => pair.Value)
                .Select(pair => (pair.Key, pair.Value))
                .ToList();
        }


        public void ResetAllScores()
        {
            scoreEventLog.Clear();
            foreach (int player in playerScores.Keys)
                playerScores[player] = 0;
        }
    }

}
