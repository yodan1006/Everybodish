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


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            {
                List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();
                if (RoundSystem.Instance != null)
                {
                    Debug.Log($"Round Player List Size: {playerInputs.Count}");
                    List<(int player, int score)> list = GlobalScoreEventSystem.GetLeaderboard();
                    Debug.Log($"Team score {GlobalScoreEventSystem.TeamScore()}");
                    bool passed = GlobalScoreEventSystem.Passed();
                    int maxScore = 0;
                    int minScore = 0;
                    teamResult[0].SetActive(passed);
                    teamResult[1].SetActive(!passed);

                    if (list.Count > 4)
                    {
                        Debug.LogError("Why are we still here? Just to suffer?", this);
                    }
                    else
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            int playerIndex = list[i].player;
                            int score = list[i].score;
                            Debug.Log($"Setting player {playerIndex} with score {score}");

                            if (maxScore < score)
                            {
                                maxScore = score;
                            }
                            if (minScore > score)
                            {
                                minScore = score;
                            }
                            playerUis[i].SetActive(true);
                            ranks[i].SetRankIcon(i);
                            sliders[i].value = score;
                            //Player input dictionary is lost on scene change.
                            // PlayerInput player = playerList[item.player];
                            //   SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                            // AnimalType animalType = selectSkin.CurrentAnimalType();
                            // icons[i].SetPlayerIcon(animalType);
                            icons[i].SetPlayerLabel(playerIndex);
                            playerScore[i].text = score.ToString();
                            i++;
                        }
                    }

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
                    Debug.LogError("ROUND IS NULL!", this);
                }
            }
        }

        public void SetIcons()
        {
            RoundSystem round = RoundSystem.Instance;
            List<PlayerInput> playerList = round.Players();
            int playerCount = playerList.Count;
            for (int i = 0; i < playerUis.Length; i++)
            {
                PlayerIcon playerIcon = playerUis[i].GetComponentInChildren<PlayerIcon>();
                if (i < playerCount)
                {
                    playerUis[i].SetActive(true);
                    PlayerInput player = playerList[i];
                    SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                    AnimalType animalType = selectSkin.CurrentAnimalType();

                    if (playerIcon != null)
                    {
                        playerIcon.SetPlayerIcon(animalType);
                        playerIcon.SetPlayerLabel(player.playerIndex);
                    }
                }
                else
                {
                    playerUis[i].SetActive(false);
                }

            }
        }

    }
}
