using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private PauseManager pauseManager;

    [Header("UI")]
    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject hudCanvas;

    [Header("Settings")]
    [SerializeField] private bool showStartMenuOnLoad = true;

    private bool isGameStarted;
    private bool isGameOver;
    private bool hasLoggedMissingPauseManager;

    private void Start()
    {
        ValidateRequiredReferences();

        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
            gameTimer.StopTimer();
        }

        if (showStartMenuOnLoad)
        {
            ShowStartMenu();
        }
        else
        {
            StartGame();
        }
    }

    private void Update()
    {
        if (!isGameStarted)
            return;

        if (isGameOver)
            return;

        if (playerHealth != null && playerHealth.IsDead)
        {
            GameOver();
        }
    }

    private void ShowStartMenu()
    {
        isGameStarted = false;
        isGameOver = false;

        RequestPause();

        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
            gameTimer.StopTimer();
        }

        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(true);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (hudCanvas != null)
            hudCanvas.SetActive(false);
    }

    public void StartGame()
    {
        isGameStarted = true;
        isGameOver = false;

        ReleasePause();

        if (gameTimer != null)
        {
            gameTimer.ResetTimer();
            gameTimer.StartTimer();
        }

        if (startMenuCanvas != null)
            startMenuCanvas.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        if (hudCanvas != null)
            hudCanvas.SetActive(true);
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameTimer != null)
        {
            gameTimer.StopTimer();
        }

        RequestPause();

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        if (pauseManager != null)
        {
            pauseManager.ClearAllPauseRequests();
        }
        else
        {
            LogMissingPauseManagerWarning();
        }

        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void ValidateRequiredReferences()
    {
        if (pauseManager == null)
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("[GameFlowManager] playerHealth가 비어 있습니다. PlayerHealth 연결이 없으면 게임오버 감지가 동작하지 않을 수 있습니다.", this);
        }

        if (gameTimer == null)
        {
            Debug.LogWarning("[GameFlowManager] gameTimer가 비어 있습니다. 타이머 UI와 시간 측정이 동작하지 않을 수 있습니다.", this);
        }

        if (startMenuCanvas == null || gameOverCanvas == null || hudCanvas == null)
        {
            Debug.LogWarning("[GameFlowManager] UI Canvas 참조가 일부 비어 있습니다. startMenuCanvas / gameOverCanvas / hudCanvas 연결을 확인하세요.", this);
        }

        if (pauseManager == null)
        {
            Debug.LogWarning("[GameFlowManager] pauseManager가 비어 있습니다. PauseManager 연결이 필요합니다.", this);
        }
    }

    private void RequestPause()
    {
        if (pauseManager != null)
        {
            pauseManager.RequestPause(this);
            return;
        }

        LogMissingPauseManagerWarning();
    }

    private void ReleasePause()
    {
        if (pauseManager != null)
        {
            pauseManager.ReleasePause(this);
            return;
        }

        LogMissingPauseManagerWarning();
    }

    private void LogMissingPauseManagerWarning()
    {
        if (hasLoggedMissingPauseManager)
            return;

        hasLoggedMissingPauseManager = true;
        Debug.LogWarning("[GameFlowManager] PauseManager가 없어 일시정지 제어를 수행할 수 없습니다. GameFlowManager.pauseManager 연결을 확인하세요.", this);
    }
}