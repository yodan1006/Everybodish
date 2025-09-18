using System.Collections.Generic;
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


        #region Unity Api

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Awake()
        {
            DontDestroyOnLoad(this);
            if (RoundSystem.Instance != null)
            {
                List<PlayerInput> playerList = RoundSystem.Instance.Players();
                Debug.Log($"Round Player List Size: {playerList.Count}");
                List<(int player, int score)> list = GlobalScoreEventSystem.GetLeaderboard();
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
                    int i = 0;
                    foreach ((int player, int score) item in list)
                    {
                        Debug.Log("Setting player " + item.player);
                        if (maxScore < item.score)
                        {
                            maxScore = item.score;
                        }
                        if (minScore > item.score)
                        {
                            minScore = item.score;
                        }
                        playerUis[i].SetActive(true);
                        ranks[i].SetRankIcon(i);
                        sliders[i].value = item.score;
                        PlayerInput player = playerList[item.player];
                        SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                        AnimalType animalType = selectSkin.CurrentAnimalType();
                        icons[i].SetPlayerIcon(animalType);
                        icons[i].SetPlayerLabel(item.player);
                        playerScore[i].text = item.score.ToString();
                        i++;
                    }
                }

                foreach (var item in sliders)
                {
                    item.gameObject.SetActive(true);
                    item.maxValue = maxScore;
                    item.minValue = minScore;
                }
            }
            else
            {
                Debug.LogError("ROUND IS NULL!", this);
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

        // Update is called once per frame
        private void Update()
        {

        }

        #endregion


        #region Main Methods

        #endregion


        #region Utils

        #endregion


        #region Private and Protected

        #endregion


    }
}
