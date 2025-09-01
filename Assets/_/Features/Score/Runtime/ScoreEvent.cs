using System;
using UnityEngine;
namespace Score.Runtime
{
    [Serializable]
    public class ScoreEvent
    {
        public float TimeStamp { get; private set; } // seconds since game start
        public int Player { get; private set; }
        public ScoreEventType EventType { get; private set; }
        public int ScoreDelta { get; private set; }
        public int? TargetPlayer { get; private set; }

        public ScoreEvent(int player, ScoreEventType eventType, int scoreDelta, int? targetPlayer = null)
        {
            TimeStamp = GameTimer.Instance.GetTime();
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