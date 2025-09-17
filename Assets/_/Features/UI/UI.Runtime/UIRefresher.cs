using System.Collections.Generic;
using Animals.Data;
using Round.Runtime;
using Score.Runtime;
using Skins.Runtime;
using Timer.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Runtime
{
    public class UIRefresher : MonoBehaviour
    {
        #region Publics
        public GameObject clock;
        public GameObject[] playerUis;
        public GameObject[] playerTexts;
        public Disabler[] upArrows;
        public Disabler[] downArrows;
        public GameObject pause;
        public float arrowDuration = 2;

        #endregion


        #region Unity Api


        // Update is called once per frame
        private void Update()
        {
            float currentTime = timer.currentTime;
            int roundDuration = round.roundDuration;
            float clockRange = 360f;
            float needleNormalizedOffset = currentTime / roundDuration;
            float needleAngle = needleNormalizedOffset * clockRange;
            needle.transform.rotation = Quaternion.Euler(0, 0, -needleAngle + needleStartRotation);
            cadranMask.fillAmount = needleNormalizedOffset;
        }
        private void Awake()
        {
            scoreSystem = GetComponent<GlobalScoreEventSystem>();
            timer = GetComponent<GameTimer>();
            round = GetComponent<RoundSystem>();
            needle = GameObject.Find("NEEDLE");
            cadranMask = GameObject.Find("FILL").GetComponent<Image>();
            needleStartRotation = needle.transform.rotation.eulerAngles.z;
            round.OnRoundStarted.AddListener(OnRoundStarted);
            round.OnRoundFinished.AddListener(OnRoundFinished);
            round.OnWarmupStarted.AddListener(OnWarmupStarted);
            round.OnWarmupFinished.AddListener(OnWarmupFinished);
            scoreSystem.OnScoreEvent.AddListener(OnScoreEvent);
        }


        private void OnScoreEvent(ScoreEvent scoreEvent)
        {
            List<PlayerInput> playerList = round.playerList;
            for (int i = 0; i < playerList.Count; i++)
            {
                Debug.Log($"i = {i},  player = {scoreEvent.Player}, playerList = {playerList.Count}, upArrows = {upArrows.Length},  downArrows = {downArrows.Length}");
                if (scoreEvent.Player == playerList[i].playerIndex)
                {
                    if (scoreEvent.ScoreDelta > 0)
                    {
                        upArrows[i].EnableForDuration(arrowDuration);
                    }
                    else if (scoreEvent.ScoreDelta < 0)
                    {
                        downArrows[i].EnableForDuration(arrowDuration);
                    }
                }

            }
        }

        private void OnWarmupFinished()
        {
            int playerCount = round.playerList.Count;
            for (int i = 0; i < playerUis.Length; i++)
            {
                PlayerIcon playerIcon = playerUis[i].GetComponentInChildren<PlayerIcon>();
                if (i < playerCount)
                {
                    playerUis[i].SetActive(true);
                    playerTexts[i].SetActive(true);
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
                    playerTexts[i].SetActive(false);
                }

            }
        }

        private void OnWarmupStarted()
        {

        }


        private void OnRoundFinished()
        {

        }

        private void OnRoundStarted()
        {

        }
        #endregion


        #region Main Methods

        #endregion


        #region Utils

        #endregion


        #region Private and Protected
        private GameTimer timer;
        private RoundSystem round;
        private GameObject needle;
        private float needleStartRotation;
        private Image cadranMask;
        private GlobalScoreEventSystem scoreSystem;
        #endregion


    }
}
