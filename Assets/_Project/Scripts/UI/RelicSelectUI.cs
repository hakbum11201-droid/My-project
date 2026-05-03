using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RelicSelectUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMeleeAutoAttack playerWeapon;
    [SerializeField] private PlayerPickupRange playerPickupRange;
    [SerializeField] private PlayerRelicEffects playerRelicEffects;

    [Header("Relic Data")]
    [SerializeField] private List<RelicData> availableRelics = new List<RelicData>();
    [SerializeField] private bool excludeOwnedRelics = true;

    [Header("UI Root")]
    [SerializeField] private GameObject relicSelectPanel;

    [Header("Texts")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text infoText;

    [Header("Buttons")]
    [SerializeField] private Button relicButton01;
    [SerializeField] private Button relicButton02;
    [SerializeField] private Button relicButton03;

    [Header("Button Texts")]
    [SerializeField] private TMP_Text relicButtonText01;
    [SerializeField] private TMP_Text relicButtonText02;
    [SerializeField] private TMP_Text relicButtonText03;

    private readonly List<RelicData> currentChoices = new List<RelicData>();
    private readonly List<RelicData> ownedRelics = new List<RelicData>();

    private bool isOpen;

    public bool IsOpen => isOpen;
    public IReadOnlyList<RelicData> OwnedRelics => ownedRelics;

    private void Awake()
    {
        AutoBindIfNeeded();
        RegisterButtonEvents();
        ClosePanelOnly();
    }

    private void AutoBindIfNeeded()
    {
        if (relicSelectPanel == null)
        {
            relicSelectPanel = transform.Find("RelicSelectPanel")?.gameObject;
        }

        if (titleText == null)
        {
            titleText = FindTMP("TitleText");
        }

        if (infoText == null)
        {
            infoText = FindTMP("InfoText");
        }

        if (relicButton01 == null)
        {
            relicButton01 = FindButton("RelicButton_01");
        }

        if (relicButton02 == null)
        {
            relicButton02 = FindButton("RelicButton_02");
        }

        if (relicButton03 == null)
        {
            relicButton03 = FindButton("RelicButton_03");
        }

        if (relicButtonText01 == null && relicButton01 != null)
        {
            relicButtonText01 = relicButton01.GetComponentInChildren<TMP_Text>(true);
        }

        if (relicButtonText02 == null && relicButton02 != null)
        {
            relicButtonText02 = relicButton02.GetComponentInChildren<TMP_Text>(true);
        }

        if (relicButtonText03 == null && relicButton03 != null)
        {
            relicButtonText03 = relicButton03.GetComponentInChildren<TMP_Text>(true);
        }

        if (playerHealth != null)
        {
            if (playerWeapon == null)
            {
                playerWeapon = playerHealth.GetComponent<PlayerMeleeAutoAttack>();
            }

            if (playerPickupRange == null)
            {
                playerPickupRange = playerHealth.GetComponent<PlayerPickupRange>();
            }

            if (playerRelicEffects == null)
            {
                playerRelicEffects = playerHealth.GetComponent<PlayerRelicEffects>();
            }
        }
    }

    private TMP_Text FindTMP(string objectName)
    {
        Transform target = transform.Find("RelicSelectPanel/" + objectName);

        if (target == null)
            return null;

        return target.GetComponent<TMP_Text>();
    }

    private Button FindButton(string objectName)
    {
        Transform target = transform.Find("RelicSelectPanel/" + objectName);

        if (target == null)
            return null;

        return target.GetComponent<Button>();
    }

    private void RegisterButtonEvents()
    {
        if (relicButton01 != null)
        {
            relicButton01.onClick.RemoveAllListeners();
            relicButton01.onClick.AddListener(() => SelectRelic(0));
        }

        if (relicButton02 != null)
        {
            relicButton02.onClick.RemoveAllListeners();
            relicButton02.onClick.AddListener(() => SelectRelic(1));
        }

        if (relicButton03 != null)
        {
            relicButton03.onClick.RemoveAllListeners();
            relicButton03.onClick.AddListener(() => SelectRelic(2));
        }
    }

    public void Open()
    {
        PickChoices();

        if (currentChoices.Count <= 0)
        {
            Debug.LogWarning("No relic choices available.");
            return;
        }

        isOpen = true;
        Time.timeScale = 0f;

        if (relicSelectPanel != null)
        {
            relicSelectPanel.SetActive(true);
        }

        UpdateTexts();
    }

    private void PickChoices()
    {
        currentChoices.Clear();

        List<RelicData> pool = new List<RelicData>();

        for (int i = 0; i < availableRelics.Count; i++)
        {
            RelicData relic = availableRelics[i];

            if (relic == null)
                continue;

            if (excludeOwnedRelics && ownedRelics.Contains(relic))
                continue;

            pool.Add(relic);
        }

        int choiceCount = Mathf.Min(3, pool.Count);

        for (int i = 0; i < choiceCount; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            currentChoices.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }
    }

    private void SelectRelic(int choiceIndex)
    {
        if (!isOpen)
            return;

        if (choiceIndex < 0 || choiceIndex >= currentChoices.Count)
            return;

        RelicData relic = currentChoices[choiceIndex];

        ApplyRelicEffect(relic);

        if (!ownedRelics.Contains(relic))
        {
            ownedRelics.Add(relic);
        }

        Debug.Log($"Relic acquired: {relic.RelicName}");

        isOpen = false;
        Time.timeScale = 1f;

        ClosePanelOnly();
    }

    private void ApplyRelicEffect(RelicData relic)
    {
        if (relic == null)
            return;

        switch (relic.RelicType)
        {
            case RelicType.IronCharm:
                if (playerHealth != null)
                {
                    playerHealth.AddDefense(relic.IntValue);
                }
                break;

            case RelicType.HunterEye:
                if (playerWeapon != null)
                {
                    playerWeapon.AddCriticalChance(relic.FloatValue);
                }
                break;

            case RelicType.GraveMagnet:
                if (playerPickupRange != null)
                {
                    playerPickupRange.ImprovePickupRange(relic.FloatValue);
                }
                break;

            case RelicType.BerserkerFang:
                if (playerRelicEffects != null)
                {
                    playerRelicEffects.ActivateBerserkerFang(
                        relic.FloatValue,
                        relic.SecondaryFloatValue
                    );
                }
                break;

            case RelicType.BloodSigil:
                if (playerRelicEffects != null)
                {
                    playerRelicEffects.ActivateBloodSigil(
                        relic.FloatValue,
                        relic.IntValue
                    );
                }
                break;
        }
    }

    private void UpdateTexts()
    {
        if (titleText != null)
        {
            titleText.text = "RELIC SELECT";
        }

        if (infoText != null)
        {
            infoText.text = "Choose 1 relic. Its effect lasts during this run.";
        }

        SetButtonText(relicButton01, relicButtonText01, 0);
        SetButtonText(relicButton02, relicButtonText02, 1);
        SetButtonText(relicButton03, relicButtonText03, 2);
    }

    private void SetButtonText(Button button, TMP_Text buttonText, int index)
    {
        bool hasChoice = index >= 0 && index < currentChoices.Count;

        if (button != null)
        {
            button.gameObject.SetActive(hasChoice);
        }

        if (!hasChoice || buttonText == null)
            return;

        RelicData relic = currentChoices[index];

        buttonText.text = relic.RelicName + "\n" + relic.Description;
    }

    private void ClosePanelOnly()
    {
        if (relicSelectPanel != null)
        {
            relicSelectPanel.SetActive(false);
        }
    }
}