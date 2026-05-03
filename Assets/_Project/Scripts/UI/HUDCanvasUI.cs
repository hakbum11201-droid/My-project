using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDCanvasUI : MonoBehaviour
{
    [Header("Targets")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerExp playerExp;
    [SerializeField] private WaveManager waveManager;
    [SerializeField] private RelicSelectUI relicSelectUI;

    [Header("HP UI")]
    [SerializeField] private Slider hpSlider;
    [SerializeField] private TMP_Text hpText;

    [Header("EXP UI")]
    [SerializeField] private Slider expSlider;
    [SerializeField] private TMP_Text expText;

    [Header("Wave UI")]
    [SerializeField] private TMP_Text waveText;

    [Header("Relic UI")]
    [SerializeField] private TMP_Text relicText;
    [SerializeField] private string relicTitle = "RELICS";
    [SerializeField] private string emptyRelicText = "RELICS\n- None";

    private readonly StringBuilder relicTextBuilder = new StringBuilder();

    private void Awake()
    {
        if (relicSelectUI == null)
        {
            relicSelectUI = FindFirstObjectByType<RelicSelectUI>();
        }
    }

    private void Update()
    {
        UpdateHpUI();
        UpdateExpUI();
        UpdateWaveUI();
        UpdateRelicUI();
    }

    private void UpdateHpUI()
    {
        if (playerHealth == null || hpSlider == null || hpText == null)
            return;

        float hpRatio = 0f;

        if (playerHealth.MaxHealth > 0)
        {
            hpRatio = (float)playerHealth.CurrentHealth / playerHealth.MaxHealth;
        }

        hpSlider.value = Mathf.Clamp01(hpRatio);
        hpText.text = $"HP {playerHealth.CurrentHealth} / {playerHealth.MaxHealth}";
    }

    private void UpdateExpUI()
    {
        if (playerExp == null || expSlider == null || expText == null)
            return;

        float expRatio = 0f;

        if (playerExp.ExpToNextLevel > 0)
        {
            expRatio = (float)playerExp.CurrentExp / playerExp.ExpToNextLevel;
        }

        expSlider.value = Mathf.Clamp01(expRatio);
        expText.text = $"LV {playerExp.Level}  EXP {playerExp.CurrentExp} / {playerExp.ExpToNextLevel}";
    }

    private void UpdateWaveUI()
    {
        if (waveManager == null || waveText == null)
            return;

        if (waveManager.IsMidBossWave())
        {
            waveText.text = $"WAVE {waveManager.CurrentWave}  MID BOSS";
        }
        else
        {
            waveText.text = $"WAVE {waveManager.CurrentWave}  NEXT {Mathf.CeilToInt(waveManager.WaveTimer)}s";
        }
    }

    private void UpdateRelicUI()
    {
        if (relicText == null)
            return;

        if (relicSelectUI == null)
        {
            relicSelectUI = FindFirstObjectByType<RelicSelectUI>();
        }

        if (relicSelectUI == null)
        {
            relicText.text = emptyRelicText;
            return;
        }

        IReadOnlyList<RelicData> ownedRelics = relicSelectUI.OwnedRelics;

        if (ownedRelics == null || ownedRelics.Count <= 0)
        {
            relicText.text = emptyRelicText;
            return;
        }

        relicTextBuilder.Clear();
        relicTextBuilder.AppendLine(relicTitle);

        for (int i = 0; i < ownedRelics.Count; i++)
        {
            RelicData relic = ownedRelics[i];

            if (relic == null)
                continue;

            relicTextBuilder.Append("- ");
            relicTextBuilder.AppendLine(relic.RelicName);
        }

        relicText.text = relicTextBuilder.ToString();
    }
}