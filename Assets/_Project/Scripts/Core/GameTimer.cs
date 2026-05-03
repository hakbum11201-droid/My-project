using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private bool isRunning;

    [Header("Time")]
    [SerializeField] private float gameplayTime;
    [SerializeField] private float realTime;
    [SerializeField] private float logTimer;

    [Header("UI")]
    [SerializeField] private TMP_Text timerText;

    [Header("Debug")]
    [SerializeField] private bool showDebugLog;
    [SerializeField] private float logInterval = 10f;

    public float GameplayTime => gameplayTime;
    public float RealTime => realTime;
    public int GameplayMinutes => Mathf.FloorToInt(gameplayTime / 60f);
    public int GameplaySeconds => Mathf.FloorToInt(gameplayTime % 60f);

    private void Update()
    {
        realTime += Time.unscaledDeltaTime;

        if (!isRunning)
        {
            UpdateTimerText();
            return;
        }

        gameplayTime += Time.deltaTime;
        logTimer += Time.unscaledDeltaTime;

        UpdateTimerText();
        UpdateDebugLog();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        gameplayTime = 0f;
        realTime = 0f;
        logTimer = 0f;
        UpdateTimerText();
    }

    public string GetFormattedGameplayTime()
    {
        int minutes = Mathf.FloorToInt(gameplayTime / 60f);
        int seconds = Mathf.FloorToInt(gameplayTime % 60f);

        return $"{minutes:00}:{seconds:00}";
    }

    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        timerText.text = GetFormattedGameplayTime();
    }

    private void UpdateDebugLog()
    {
        if (!showDebugLog)
            return;

        if (logTimer < logInterval)
            return;

        logTimer = 0f;

        Debug.Log($"[GameTimer] Gameplay: {GetFormattedGameplayTime()} / Real: {realTime:F1}s");
    }
}