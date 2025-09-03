using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    public static GameTimer Instance;
    private bool hasStarted = false;
    private bool isStopped = false;
    public float currentTime = 0;
    public bool IsRunning => hasStarted && !isStopped;
    public bool IsStopped => isStopped;
    public bool IsStarted => hasStarted;
    public UnityEvent onTimerStarted;
    public UnityEvent onTimerStopped;
    public UnityEvent onTimerPaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("More than one gameTimer instanced! If you want to access the game timer, use GameTimer.Instance.", this);
        }

    }

    private void Update()
    {
        if (hasStarted && !isStopped)
        {
            currentTime += Time.deltaTime;
        }
    }

    public float ElapsedTime
    {
        get
        {
            return currentTime;
        }
    }

    /// <summary>
    /// Starts or restarts the timer.
    /// </summary>
    public void StartGameTimer()
    {
        currentTime = 0f;
        hasStarted = true;
        isStopped = false;
        Debug.Log("Game timer started.");
    }

    /// <summary>
    /// Stops the timer and freezes the time.
    /// </summary>
    public void StopGameTimer()
    {
        if (hasStarted && !isStopped)
        {
            isStopped = true;
            Debug.Log($"Game timer stopped at {currentTime:F2} seconds.");
        }
    }

    /// <summary>
    /// Resets the timer to zero and stops it.
    /// </summary>
    public void ResetGameTimer()
    {
        currentTime = 0f;
        hasStarted = false;
        isStopped = false;
        Debug.Log("Game timer reset.");
    }

    /// <summary>
    /// Returns current elapsed time.
    /// </summary>
    public float GetTime()
    {
        return ElapsedTime;
    }
}
