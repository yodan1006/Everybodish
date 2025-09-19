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
using UnityEngine.PlayerLoop;
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
               

                if (RoundSystem.Instance != null)
                {
                    //get unsorted player inputs
                    List<PlayerInput> playerInputs = FindObjectsByType<PlayerInput>(FindObjectsSortMode.InstanceID).ToList();
                    Debug.Log($"Round Player List Size: {playerInputs.Count}");
                    //Get sorted player scores
                    List<(int player, int score)> list = GlobalScoreEventSystem.GetLeaderboard();
                    //Get Team score and whether or not they had their quota fullfilled
                    Debug.Log($"Team score {GlobalScoreEventSystem.TeamScore()}");
                    bool passed = GlobalScoreEventSystem.Passed();
                    //We will need to find the min score and max score
                    int maxScore = 0;
                    int minScore = 0;

                    //Activate correct win/lose label
                    teamResult[0].SetActive(passed);
                    teamResult[1].SetActive(!passed);

                    //Sanity check
                    if (playerInputs.Count > 4)
                    {
                        Debug.LogError("Why are we still here? Just to suffer?", this);
                    }
                    else
                    {
                        //playerInputs are unsorted => issue
                        for (int i = 0; i < playerInputs.Count; i++)
                        {
                            //get necessary info
                            int playerIndex = playerInputs[i].playerIndex;
                            PlayerInput player = playerInputs[i];
                            int score = list[i].score;

                            Debug.Log($"Setting player {playerIndex} with score {score}");
                            //update max/min score
                            if (maxScore < score)
                            {
                                maxScore = score;
                            }
                            if (minScore > score)
                            {
                                minScore = score;
                           }
                            //Update the ui
                            playerUis[i].SetActive(true);
                            ranks[i].SetRankIcon(i);
                           sliders[i].value = score;
                           
                            //Set player icon based on selected skin in player
                            SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                            AnimalType animalType = selectSkin.CurrentAnimalType();
                            //set correct player icon
                            icons[i].SetPlayerIcon(animalType);
                            //set player 1/2/3/a
                            icons[i].SetPlayerLabel(playerIndex);
                            //Set score label
                            playerScore[i].text = score.ToString();
                        }
                    }

                    //push the score slider back a little
                    if(minScore< 10)
                    {
                        minScore = -10;
                    }

                    //update sliders
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
    }
}
