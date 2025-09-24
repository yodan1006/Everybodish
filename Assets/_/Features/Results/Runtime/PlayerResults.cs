using System.Collections.Generic;
using System.Linq;
using Animals.Data;
using Round.Runtime;
using Score.Runtime;
using Skins.Runtime;
using TMPro;
using UI.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Results.Runtime
{
    public class PlayerResults : MonoBehaviour
    {
        #region Publics
        public GameObject[] playerUis;
        public PlayerIcon[] icons;
        public Slider[] sliders;
        public TextMeshProUGUI[] playerScore;
        public PlayerRank[] ranks;
        public GameObject[] teamResult;
        #endregion

        private void Awake()
        {
            if (RoundSystem.Instance != null)
            {
                // Get all PlayerInput objects and sort them by lobby slot index
                List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.None)
                    .OrderBy(p => p.GetComponentInChildren<SelectSkin>().GetSlotIndex())
                    .ToList();

                Debug.Log($"Sorted Player List by Index: {string.Join(", ", playerInputs.Select(p => p.playerIndex))}");

                // Get sorted player scores from the leaderboard (already sorted by rank/score)
                List<(int player, int score)> list = GlobalScoreEventSystem.GetLeaderboard();

                Debug.Log($"Team score {GlobalScoreEventSystem.TeamScore()}");
                bool passed = GlobalScoreEventSystem.Passed();

                int maxScore = 0;
                int minScore = 0;

                // Activate win/lose labels
                teamResult[0].SetActive(passed);
                teamResult[1].SetActive(!passed);

                // Build dictionary: playerIndex → rank (accounting for ties)
                Dictionary<int, int> playerRanks = new();
                int currentRank = 0;
                int previousScore = int.MinValue;
                int playersWithSameScore = 0;

                for (int i = 0; i < list.Count; i++)
                {
                    var (playerIndex, score) = list[i];

                    if (score != previousScore)
                    {
                        currentRank += playersWithSameScore;
                        playersWithSameScore = 1;
                        previousScore = score;
                    }
                    else
                    {
                        playersWithSameScore++;
                    }

                    playerRanks[playerIndex] = currentRank;
                }

                if (playerInputs.Count > 4)
                {
                    Debug.LogError("Why are we still here? Just to suffer?", this);
                }
                else
                {
                    for (int i = 0; i < playerInputs.Count; i++)
                    {
                        PlayerInput player = playerInputs[i];
                        int playerIndex = player.playerIndex;

                        // Match score from leaderboard
                        int score = list.FirstOrDefault(s => s.player == playerIndex).score;

                        Debug.Log($"Setting player {playerIndex} with score {score}");

                        if (score > maxScore) maxScore = score;
                        if (score < minScore) minScore = score;

                        playerUis[i].SetActive(true);
                        int leaderboardRank = 0;
                        // Get tie-aware rank from dictionary
                        if (playerIndex < playerRanks.Count)
                        {
                            leaderboardRank = playerRanks[playerIndex];
                        }

                        ranks[i].SetRankIcon(leaderboardRank);

                        sliders[i].value = score;

                        SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                        AnimalType animalType = selectSkin.CurrentAnimalType();
                        int slotIndex = selectSkin.GetSlotIndex();

                        icons[i].SetPlayerIcon(animalType, false);
                        icons[i].SetPlayerLabel(slotIndex);
                        playerScore[i].text = score.ToString();
                    }
                }

                if (minScore < 10)
                {
                    minScore = -2;
                }

                // Update slider min/max values
                for (int i = 0; i < sliders.Length; i++)
                {
                    if (i < playerInputs.Count)
                    {
                        sliders[i].gameObject.SetActive(true);
                        sliders[i].maxValue = maxScore;
                        sliders[i].minValue = minScore;
                    }
                }
            }
            else
            {
                //  Debug.LogError("ROUND IS NULL!", this);
            }
        }
    }
}