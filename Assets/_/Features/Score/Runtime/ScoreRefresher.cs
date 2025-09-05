using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Score.Runtime
{
    [RequireComponent(typeof(GlobalScoreEventSystem))]
    public class ScoreRefresher : MonoBehaviour
    {
        private GlobalScoreEventSystem scoreSystem;
        private Round round;
        [SerializeField] private TextMeshProUGUI player1Scoreboard;
        [SerializeField] private TextMeshProUGUI player2Scoreboard;
        [SerializeField] private TextMeshProUGUI player3Scoreboard;
        [SerializeField] private TextMeshProUGUI player4Scoreboard;
        private readonly List<TextMeshProUGUI> playerScoreboardList = new();
        private void Awake()
        {
            scoreSystem = GetComponent<GlobalScoreEventSystem>();
            round = GetComponent<Round>();
            scoreSystem.OnScoresChanged.AddListener(OnScoresChanged);
            scoreSystem.OnScoreEvent.AddListener(OnScoreEvent);
            playerScoreboardList.Add(player1Scoreboard);
            playerScoreboardList.Add(player2Scoreboard);
            playerScoreboardList.Add(player3Scoreboard);
            playerScoreboardList.Add(player4Scoreboard);
        }

        private void OnScoreEvent(ScoreEvent scoreEvent)
        {
            Debug.Log("OnScoreEvent");
        }

        private void OnScoresChanged()
        {
            Debug.Log("OnScoresChanged");
            List<PlayerInput> playerList = round.playerList;
            int playerCount = playerList.Count;
            int playerScoreboardCount = playerScoreboardList.Count;
            for (int i = 0; i < playerScoreboardList.Count; i++)
            {
                if (i < playerList.Count)
                {
                    playerScoreboardList[i].enabled = true;
                    playerScoreboardList[i].SetText(scoreSystem.PlayerScores.GetValueOrDefault(playerList[i].playerIndex).ToString());
                }
                else
                {
                    playerScoreboardList[i].enabled = false;
                }
            }
        }
    }
}
