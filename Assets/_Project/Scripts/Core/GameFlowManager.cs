using UnityEngine;
using UnityEngine.SceneManagement;

public class GameFlowManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private GameTimer gameTimer;

    [Header("UI")]
    [SerializeField] private GameObject startMenuCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject hudCanvas;

    [Header("Settings")]
    [SerializeField] private bool showStartMenuOnLoad = true;

    private bool isGameStarted;
    private bool isGameOver;

    private void Start()
    {
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

        Time.timeScale = 0f;

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

        Time.timeScale = 1f;

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

        Time.timeScale = 0f;

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

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
}