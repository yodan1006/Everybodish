using Timer.Runtime;
using TMPro;
using UnityEngine;

namespace Round.Runtime
{
    [RequireComponent(typeof(GameTimer))]
    [RequireComponent(typeof(RoundSystem))]
    public class TimerRefresher : MonoBehaviour
    {
        private GameTimer timer;
        private RoundSystem round;
        [SerializeField] private TextMeshProUGUI roundTimeleft;
        [SerializeField] private TextMeshProUGUI warmupTimeleft;
        private void Awake()
        {
            timer = GetComponent<GameTimer>();
            round = GetComponent<RoundSystem>();
            round.OnRoundStarted.AddListener(OnRoundStarted);
            round.OnRoundFinished.AddListener(OnRoundFinished);
            round.OnWarmupStarted.AddListener(OnWarmupStarted);
            round.OnWarmupFinished.AddListener(OnWarmupFinished);
        }

        private void OnWarmupStarted()
        {
            warmupTimeleft.enabled = true;
            roundTimeleft.enabled = false;
        }
        private void OnWarmupFinished()
        {
            warmupTimeleft.enabled = false;
        }

        private void OnRoundStarted()
        {
            roundTimeleft.enabled = true;
        }

        private void OnRoundFinished()
        {
            roundTimeleft.enabled = false;
        }

        private void Update()
        {
            float v = Mathf.Ceil(round.WarmupTimeDelta -1);
            if (warmupTimeleft.enabled)
            {
                if (v > 0)
                {
                    warmupTimeleft.SetText(v.ToString("0"));
                }
                else
                {
                    warmupTimeleft.SetText("GO!");
                }

            }

            if (roundTimeleft.enabled)
            {
                roundTimeleft.SetText(timer.GetFormatedTimeLeft(round.roundDuration));
            }
        }
    }
}
