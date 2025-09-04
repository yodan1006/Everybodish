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

        private readonly List<ScoreEvent> scoreEventLog = new();
        private readonly Dictionary<int, int> playerScores = new();

        public IReadOnlyList<ScoreEvent> ScoreEventLog => scoreEventLog.AsReadOnly();

        public Dictionary<int, int> PlayerScores => playerScores;
        [Header("Score Values")]
        public int joinedGameDelta;
        public int CookedItemDelta;
        public int ServedItemDelta;
        public int BurnedItemDelta;
        public int ThrewAwayItemDelta;
        public int BonusTipDelta;
        public int PenaltyDelta;
        public int ComboAchievedDelta;
        public int PlayerKilledDelta;
        public int FoodPoisonedDelta;
        [Header("Score Events")]
        public ScoreEventUnityEvent OnScoreEvent = new();
        public UnityEvent OnScoresChanged = new();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogError("More than one GlobalScoreEventSystem instanced! If you want to access the game timer, use GlobalScoreEventSystem.Instance.", this);
            }


            DontDestroyOnLoad(gameObject);

            foreach (int player in playerScores.Keys)
            {
                playerScores[player] = 0;
            }
        }

        private int GetScoreDelta(ScoreEventType eventType)
        {
            int delta = 0;
            switch (eventType)
            {
                case ScoreEventType.JoinedGame:
                    delta = joinedGameDelta;
                    break;
                case ScoreEventType.CookedDish:
                    delta = CookedItemDelta;
                    break;
                case ScoreEventType.ServedDish:
                    delta = ServedItemDelta;
                    break;
                case ScoreEventType.BurnedDish:
                    delta = BurnedItemDelta;
                    break;
                case ScoreEventType.ThrewAwayItem:
                    delta = ThrewAwayItemDelta;
                    break;
                case ScoreEventType.BonusTip:
                    delta = BonusTipDelta;
                    break;
                case ScoreEventType.Penalty:
                    delta = PenaltyDelta;
                    break;
                case ScoreEventType.PlayerKilled:
                    delta = PlayerKilledDelta;
                    break;
                case ScoreEventType.FoodPoisoned:
                    delta = FoodPoisonedDelta;
                    break;
            }
            return delta;
        }

        public void RegisterScoreEvent(int player, ScoreEventType eventType, int? targetPlayer = null)
        {
            int scoreDelta = GetScoreDelta(eventType);
            var scoreEvent = new ScoreEvent(player, eventType, scoreDelta, targetPlayer);
            scoreEventLog.Add(scoreEvent);

            if (playerScores.ContainsKey(player))
            {
                playerScores[player] += scoreDelta;
            }
            else
            {
                playerScores[player] = scoreDelta;
            }
            Debug.Log($"[ScoreEvent] {player} - {eventType} - Score: {scoreDelta}" +
                      (targetPlayer.HasValue ? $" (Target: {targetPlayer.Value})" : "") +
                      $" @ {scoreEvent.TimeStamp}");

            OnScoreEvent.Invoke(scoreEvent);
            OnScoresChanged.Invoke();
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
            {
                playerScores[player] = 0;
            }
            OnScoresChanged.Invoke();
        }
    }

}
