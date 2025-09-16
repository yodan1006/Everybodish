using System.Collections.Generic;
using Score.Runtime;
using TMPro;
using UI.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace Results.Runtime
{
    public class PlayerResults : MonoBehaviour
    {
        #region Publics
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
                    ranks[i].SetPlayerIcon(i) ;
                    sliders[i].value = item.Value;
                    i++;
                }
            }

            foreach (var item in sliders)
            {
                item.maxValue = maxScore;
                item.minValue = minScore;
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
