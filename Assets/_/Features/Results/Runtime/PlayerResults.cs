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
        private void Start()
        {
            List<PlayerInput> playerList = RoundSystem.Instance.playerList;
            Dictionary<int, int> playerScores = GlobalScoreEventSystem.PlayerScores;
            bool passed = GlobalScoreEventSystem.Passed();
            int maxScore = 0;
            int minScore = 0;
            teamResult[0].SetActive(passed);
            teamResult[1].SetActive(!passed);
            if (playerScores.Count > 4)
            {
                Debug.LogError("Why are we still here? Just to suffer?", this);
            }
            else
            {
                int i = 0;
                
                foreach (var item in playerScores)
                {
                    if (maxScore < item.Value)
                    {
                        maxScore = item.Value;
                    }
                    if (minScore > item.Value)
                    {
                        minScore = item.Value;
                    }
                    ranks[i].SetRankIcon(i) ;
                    sliders[i].value = item.Value;
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


        public void SetIcons()
        {
            RoundSystem round = RoundSystem.Instance;
            int playerCount = round.playerList.Count;
            for (int i = 0; i < playerUis.Length; i++)
            {
                PlayerIcon playerIcon = playerUis[i].GetComponentInChildren<PlayerIcon>();
                if (i < playerCount)
                {
                    playerUis[i].SetActive(true);
            //        playerTexts[i].SetActive(true);
                    PlayerInput player = round.playerList[i];
                    SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                    AnimalType animalType = selectSkin.CurrentAnimalType();

                    if (playerIcon != null)
                    {
                        playerIcon.SetPlayerIcon(animalType);
                    }
                }
                else
                {
                    playerUis[i].SetActive(false);
               //     playerTexts[i].SetActive(false);
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
