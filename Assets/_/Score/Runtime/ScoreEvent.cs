using System;
using UnityEngine;
namespace Score.Runtime
{
    [Serializable]
    public class ScoreEvent
    {
        public float TimeStamp { get; private set; } // seconds since game start
        public PlayerID Player { get; private set; }
        public ScoreEventType EventType { get; private set; }
        public int ScoreDelta { get; private set; }
        public PlayerID? TargetPlayer { get; private set; }

        public ScoreEvent(PlayerID player, ScoreEventType eventType, int scoreDelta, PlayerID? targetPlayer = null)
        {
            TimeStamp = GameTimer.Instance.GetTimestamp();
            Player = player;
            EventType = eventType;
            ScoreDelta = scoreDelta;
            TargetPlayer = targetPlayer;
        }

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            float seconds = timeInSeconds % 60f;
            return $"{minutes:00}:{seconds:00.0}";
        }
    }
}