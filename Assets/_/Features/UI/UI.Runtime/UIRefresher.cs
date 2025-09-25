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
        public PlayerIcon[] playerIcons;
        public Disabler[] upArrows;
        public Disabler[] downArrows;
        public List<SelectSkin> selectSkins = new();
        public GameObject pause;
        public float arrowDuration = 2;
        private List<PlayerInput> playerList;
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
        private void Start()
        {
            scoreSystem = GetComponent<GlobalScoreEventSystem>();
            timer = GetComponent<GameTimer>();
            round = RoundSystem.Instance;
            needle = GameObject.Find("NEEDLE");
            cadranMask = GameObject.Find("FILL").GetComponent<Image>();
            needleStartRotation = needle.transform.rotation.eulerAngles.z;
            round.OnGameplayStarted.AddListener(OnRoundStarted);
            round.OnWarmupStarted.AddListener(OnWarmupStarted);
            round.OnWarmupFinished.AddListener(OnWarmupFinished);
            scoreSystem.OnScoreEvent.AddListener(OnScoreEvent);
        }

        private void OnScoreEvent(ScoreEvent scoreEvent)
        {
            List<(int player, int score)> leaderboard = GlobalScoreEventSystem.GetLeaderboard();
            int firstPlayerIndex = leaderboard[0].player;

            Debug.Log(playerList.Count);
            Debug.Log(selectSkins.Count);
            for (int i = 0; i < playerList.Count; i++)
            {
                AnimalType animalType = selectSkins[i].CurrentAnimalType();
                int playerIndex = playerList[i].playerIndex;
                bool isWinning = playerIndex == firstPlayerIndex;
                //update all icons
                playerIcons[i].SetPlayerIcon(animalType, isWinning);
                // Debug.Log($"i = {i},  player = {scoreEvent.Player}, playerList = {playerList.Count}, upArrows = {upArrows.Length},  downArrows = {downArrows.Length}");
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

        private void OnWarmupStarted()
        {
            Debug.Log("Warmup started");
            //Disable player ui
            foreach (var item in playerIcons)
            {
                item.gameObject.SetActive(false);
            }
            // Fill select skin component list


        }

        private void OnWarmupFinished()
        {
            Debug.Log("Warmup finished");
            //Enable player ui
            foreach (var item in playerIcons)
            {
                item.gameObject.SetActive(true);
            }
            playerList = round.Players();
            foreach (var item in playerList)
            {
                SelectSkin selectSkin = item.GetComponent<SelectSkin>();
                selectSkins.Add(selectSkin);
            }
        }
        private void OnRoundStarted()
        {
            Debug.Log("Round started, updating ui");

            int playerCount = playerList.Count;
            Debug.Log($"Players in game : {playerCount}");
            Debug.Log($"Players icons count : {selectSkins.Count}");
            for (int i = 0; i < playerIcons.Length; i++)
            {
                PlayerIcon playerIcon = playerIcons[i];
                if (i < playerCount)
                {
                    playerIcons[i].gameObject.SetActive(true);

                    AnimalType animalType = selectSkins[i].CurrentAnimalType();

                    if (playerIcon != null)
                    {
                        playerIcon.SetPlayerIcon(animalType, false);
                        playerIcon.SetPlayerLabel(i);
                    }
                }
                else
                {
                    playerIcons[i].gameObject.SetActive(false);
                }

            }
            // Debug.LogError("Ui initialization complete");
        }
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
