using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDDebugUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerExp playerExp;
    [SerializeField] private WaveManager waveManager;

    [Header("Layout")]
    [SerializeField] private int x = 20;
    [SerializeField] private int y = 20;
    [SerializeField] private int width = 260;
    [SerializeField] private int height = 24;

    private void OnGUI()
    {
        DrawHealth();
        DrawExp();
        DrawWave();
        DrawGameOver();
    }

    private void DrawHealth()
    {
        if (playerHealth == null)
            return;

        float hpRatio = 0f;

        if (playerHealth.MaxHealth > 0)
        {
            hpRatio = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
        }

        GUI.Box(new Rect(x, y, width, height), "");
        GUI.Box(new Rect(x, y, width * hpRatio, height), "");

        GUI.Label(
            new Rect(x + 8, y + 3, width, height),
            $"HP {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}"
        );
    }

    private void DrawExp()
    {
        if (playerExp == null)
            return;

        int expY = y + height + 8;

        float expRatio = 0f;

        if (playerExp.ExpToNextLevel > 0)
        {
            expRatio = (float)playerExp.CurrentExp / playerExp.ExpToNextLevel;
        }

        GUI.Box(new Rect(x, expY, width, height), "");
        GUI.Box(new Rect(x, expY, width * expRatio, height), "");

        GUI.Label(
            new Rect(x + 8, expY + 3, width, height),
            $"LV {playerExp.Level}  EXP {playerExp.CurrentExp} / {playerExp.ExpToNextLevel}"
        );
    }

    private void DrawWave()
    {
        if (waveManager == null)
            return;

        int waveY = y + (height + 8) * 2;

        string label = waveManager.IsMidBossWave()
            ? $"WAVE {waveManager.CurrentWave}  MID BOSS"
            : $"WAVE {waveManager.CurrentWave}  NEXT {Mathf.CeilToInt(waveManager.WaveTimer)}s";

        GUI.Box(new Rect(x, waveY, width, height), label);
    }

    private void DrawGameOver()
    {
        if (playerHealth == null || !playerHealth.IsDead)
            return;

        int boxWidth = 320;
        int boxHeight = 160;
        int boxX = (Screen.width - boxWidth) / 2;
        int boxY = (Screen.height - boxHeight) / 2;

        GUI.Box(new Rect(boxX, boxY, boxWidth, boxHeight), "GAME OVER");

        GUI.Label(
            new Rect(boxX + 40, boxY + 45, boxWidth - 80, 30),
            "플레이어가 사망했습니다."
        );

        if (GUI.Button(new Rect(boxX + 60, boxY + 95, boxWidth - 120, 40), "다시 시작"))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}