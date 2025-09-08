using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Score.Runtime
{
    [DisallowMultipleComponent]
    public class GlobalScoreEventSystem : MonoBehaviour
    {
        public static GlobalScoreEventSystem Instance { get; private set; }

        // Static data structures
        private static readonly List<ScoreEvent> scoreEventLog = new();
        private static readonly Dictionary<int, int> playerScores = new();

        public static IReadOnlyList<ScoreEvent> ScoreEventLog => scoreEventLog.AsReadOnly();
        public static Dictionary<int, int> PlayerScores => playerScores;

        // Score values per event type - non-static, configurable via Inspector
        [Header("Score Values")]
        public int joinedGameDelta;
        public int preparedIngredientDelta;
        public int CookedItemDelta;
        public int ServedItemDelta;
        public int BurnedItemDelta;
        public int ThrewAwayItemDelta;
        public int BonusTipDelta;
        public int PenaltyDelta;
        public int ComboAchievedDelta;
        public int PlayerKilledDelta;
        public int PlayerDiedDelta;
        public int FoodPoisonedDelta;

        [Header("Score Events")]
        public ScoreEventUnityEvent OnScoreEvent = new();
        public UnityEvent OnScoresChanged = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogError("More than one GlobalScoreEventSystem instanced! Use GlobalScoreEventSystem.Instance.", this);
                Destroy(gameObject);
            }
        }

        private static int GetScoreDelta(ScoreEventType eventType)
        {
            if (Instance == null)
            {
                Debug.LogWarning("GlobalScoreEventSystem.Instance is null when accessing score deltas.");
                return 0;
            }

            return eventType switch
            {
                ScoreEventType.JoinedGame => Instance.joinedGameDelta,
                ScoreEventType.CookedDish => Instance.CookedItemDelta,
                ScoreEventType.ServedDish => Instance.ServedItemDelta,
                ScoreEventType.BurnedDish => Instance.BurnedItemDelta,
                ScoreEventType.ThrewAwayItem => Instance.ThrewAwayItemDelta,
                ScoreEventType.BonusTip => Instance.BonusTipDelta,
                ScoreEventType.Penalty => Instance.PenaltyDelta,
                ScoreEventType.PlayerKilled => Instance.PlayerKilledDelta,
                ScoreEventType.PlayerDied => Instance.PlayerDiedDelta,
                ScoreEventType.FoodPoisoned => Instance.FoodPoisonedDelta,
                ScoreEventType.PreparedIngredient => Instance.preparedIngredientDelta,
                _ => throw new System.NotImplementedException()
            };
        }

        public static void RegisterScoreEvent(int player, ScoreEventType eventType, int? targetPlayer = null)
        {
            int scoreDelta = GetScoreDelta(eventType);
            var scoreEvent = new ScoreEvent(player, eventType, scoreDelta, targetPlayer);
            scoreEventLog.Add(scoreEvent);

            if (playerScores.ContainsKey(player))
                playerScores[player] += scoreDelta;
            else
                playerScores[player] = scoreDelta;

            Debug.Log($"[ScoreEvent] player {player} - {eventType} - Score: {scoreDelta}" +
                      (targetPlayer.HasValue ? $" (Target: {targetPlayer.Value})" : "") +
                      $" @ {scoreEvent.TimeStamp}");

            Instance?.OnScoreEvent?.Invoke(scoreEvent);
            Instance?.OnScoresChanged?.Invoke();
        }

        public static int GetScore(int player)
        {
            return playerScores.TryGetValue(player, out int score) ? score : 0;
        }

        public static List<(int player, int score)> GetLeaderboard()
        {
            return playerScores
                .OrderByDescending(pair => pair.Value)
                .Select(pair => (pair.Key, pair.Value))
                .ToList();
        }

        public static void ResetAllScores()
        {
            scoreEventLog.Clear();
            foreach (var key in playerScores.Keys.ToList())
                playerScores[key] = 0;

            Instance?.OnScoresChanged?.Invoke();
        }
    }
}