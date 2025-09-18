using UnityEngine;
using UnityEngine.Events;

namespace Timer.Runtime
{
    [DisallowMultipleComponent]
    public class GameTimer : MonoBehaviour
    {
        public static GameTimer Instance { get; private set; }

        private bool hasStarted = false;
        private bool isStopped = false;
        public float currentTime = 0;

        public bool IsRunning => hasStarted && !isStopped;
        public bool IsStopped => isStopped;
        public bool IsStarted => hasStarted;

        public UnityEvent onTimerStarted = new();
        public UnityEvent onTimerStopped = new();
        public UnityEvent onTimerPaused = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogError("Multiple GameTimer instances detected! Destroying duplicate.", this);
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: persist across scenes
        }

        private void Update()
        {
            if (hasStarted && !isStopped)
            {
                currentTime += Time.deltaTime;
            }
        }

        public void StartGameTimer()
        {
            currentTime = 0f;
            hasStarted = true;
            isStopped = false;
            Debug.Log("Game timer started.");
            onTimerStarted?.Invoke();
        }

        public void StopGameTimer()
        {
            if (hasStarted && !isStopped)
            {
                isStopped = true;
                Debug.Log($"Game timer stopped at {currentTime:F2} seconds.");
                onTimerStopped?.Invoke();
            }
        }

        public void ResetGameTimer()
        {
            currentTime = 0f;
            hasStarted = false;
            isStopped = false;
            Debug.Log("Game timer reset.");
        }

        public float GetTime() => currentTime;

        public string GetFormattedTime() => FormatTime(currentTime);

        public string GetFormattedTimeLeft(float targetTime) => FormatTime(targetTime - currentTime);

        public static string FormatTime(float timeInSeconds)
        {
            int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
            float seconds = timeInSeconds % 60f;
            return $"{minutes:00}:{seconds:00.0}";
        }
    }
}