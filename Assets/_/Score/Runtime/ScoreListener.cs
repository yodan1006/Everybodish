using UnityEngine;

namespace Score.Runtime
{


    public class ScoreListener : MonoBehaviour
    {
        private void OnEnable()
        {
            if (GlobalScoreEventSystem.Instance != null)
            {
                GlobalScoreEventSystem.Instance.OnScoreEvent.AddListener(OnScoreEventReceived);
            }
        }

        private void OnDisable()
        {
            if (GlobalScoreEventSystem.Instance != null)
            {
                GlobalScoreEventSystem.Instance.OnScoreEvent.RemoveListener(OnScoreEventReceived);
            }
        }

        private void OnScoreEventReceived(ScoreEvent scoreEvent)
        {
            Debug.Log($"[Listener] {scoreEvent.Player} gained {scoreEvent.ScoreDelta} from {scoreEvent.EventType}");
            // Do something, e.g. update UI or play sound
        }
    }

}
