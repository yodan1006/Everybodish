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
        private void Start()
        {
            scoreSystem = GetComponent<GlobalScoreEventSystem>();
            timer = GetComponent<GameTimer>();
            round = RoundSystem.Instance;
            needle = GameObject.Find("NEEDLE");
            cadranMask = GameObject.Find("FILL").GetComponent<Image>();
            needleStartRotation = needle.transform.rotation.eulerAngles.z;
            round.OnGameplayStarted.AddListener(OnRoundStarted);
            round.OnGameplayFinished.AddListener(OnRoundFinished);
            round.OnWarmupStarted.AddListener(OnWarmupStarted);
            round.OnWarmupFinished.AddListener(OnWarmupFinished);
            scoreSystem.OnScoreEvent.AddListener(OnScoreEvent);
        }

        private void OnScoreEvent(ScoreEvent scoreEvent)
        {
            List<PlayerInput> playerList = round.Players();
            for (int i = 0; i < playerList.Count; i++)
            {
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
            //Disable player ui
            foreach (var item in playerIcons)
            {
                item.gameObject.SetActive(false);
            }

        }

        private void OnWarmupFinished()
        {
            //Disable player ui
            foreach (var item in playerIcons)
            {
                item.gameObject.SetActive(true);
            }

        }
        private void OnRoundStarted()
        {
            Debug.Log("Round started, updating ui");
            List<PlayerInput> players = round.Players();
            int playerCount = players.Count;
            Debug.Log($"Players in game : {playerCount}");
            Debug.Log($"Players icons count : {playerIcons.Length}");
            for (int i = 0; i < playerIcons.Length; i++)
            {
                PlayerIcon playerIcon = playerIcons[i];
                if (i < playerCount)
                {
                    playerIcons[i].gameObject.SetActive(true);
                    PlayerInput player = players[i];
                    SelectSkin selectSkin = player.GetComponent<SelectSkin>();
                    AnimalType animalType = selectSkin.CurrentAnimalType();

                    if (playerIcon != null)
                    {
                        playerIcon.SetPlayerIcon(animalType);
                        playerIcon.SetPlayerLabel(i);
                    }
                }
                else
                {
                    playerIcons[i].gameObject.SetActive(false);
                }

            }
            Debug.LogError("Ui initialization complete");
        }

        private void OnRoundFinished()
        {

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
