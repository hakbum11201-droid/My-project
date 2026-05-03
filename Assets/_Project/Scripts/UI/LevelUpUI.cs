using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMeleeAutoAttack playerWeapon;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerPickupRange playerPickupRange;

    [Header("Upgrade Data")]
    [SerializeField] private List<UpgradeData> availableUpgrades = new List<UpgradeData>();

    [Header("UI Root")]
    [SerializeField] private GameObject levelUpPanel;

    [Header("Texts")]
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text pendingText;

    [Header("Buttons")]
    [SerializeField] private Button damageButton;
    [SerializeField] private Button attackSpeedButton;
    [SerializeField] private Button attackRangeButton;

    [Header("Button Texts")]
    [SerializeField] private TMP_Text damageButtonText;
    [SerializeField] private TMP_Text attackSpeedButtonText;
    [SerializeField] private TMP_Text attackRangeButtonText;

    private readonly List<UpgradeData> currentChoices = new List<UpgradeData>();

    private int pendingLevelUps;
    private bool isOpen;

    private void Awake()
    {
        AutoBindIfNeeded();
        RegisterButtonEvents();
        ClosePanelOnly();
    }

    private void AutoBindIfNeeded()
    {
        if (levelUpPanel == null)
        {
            levelUpPanel = transform.Find("LevelUpPanel")?.gameObject;
        }

        if (titleText == null)
        {
            titleText = FindTMP("TitleText");
        }

        if (pendingText == null)
        {
            pendingText = FindTMP("PendingText");
        }

        if (damageButton == null)
        {
            damageButton = FindButton("DamageButton");
        }

        if (attackSpeedButton == null)
        {
            attackSpeedButton = FindButton("AttackSpeedButton");
        }

        if (attackRangeButton == null)
        {
            attackRangeButton = FindButton("AttackRangeButton");
        }

        if (damageButtonText == null && damageButton != null)
        {
            damageButtonText = damageButton.GetComponentInChildren<TMP_Text>(true);
        }

        if (attackSpeedButtonText == null && attackSpeedButton != null)
        {
            attackSpeedButtonText = attackSpeedButton.GetComponentInChildren<TMP_Text>(true);
        }

        if (attackRangeButtonText == null && attackRangeButton != null)
        {
            attackRangeButtonText = attackRangeButton.GetComponentInChildren<TMP_Text>(true);
        }

        if (playerController == null && playerHealth != null)
        {
            playerController = playerHealth.GetComponent<PlayerController>();
        }

        if (playerController == null && playerWeapon != null)
        {
            playerController = playerWeapon.GetComponent<PlayerController>();
        }

        if (playerPickupRange == null && playerHealth != null)
        {
            playerPickupRange = playerHealth.GetComponent<PlayerPickupRange>();
        }

        if (playerPickupRange == null && playerController != null)
        {
            playerPickupRange = playerController.GetComponent<PlayerPickupRange>();
        }
    }

    private TMP_Text FindTMP(string objectName)
    {
        Transform target = transform.Find("LevelUpPanel/" + objectName);

        if (target == null)
        {
            return null;
        }

        return target.GetComponent<TMP_Text>();
    }

    private Button FindButton(string objectName)
    {
        Transform target = transform.Find("LevelUpPanel/" + objectName);

        if (target == null)
        {
            return null;
        }

        return target.GetComponent<Button>();
    }

    private void RegisterButtonEvents()
    {
        if (damageButton != null)
        {
            damageButton.onClick.RemoveAllListeners();
            damageButton.onClick.AddListener(() => ChooseUpgrade(0));
        }

        if (attackSpeedButton != null)
        {
            attackSpeedButton.onClick.RemoveAllListeners();
            attackSpeedButton.onClick.AddListener(() => ChooseUpgrade(1));
        }

        if (attackRangeButton != null)
        {
            attackRangeButton.onClick.RemoveAllListeners();
            attackRangeButton.onClick.AddListener(() => ChooseUpgrade(2));
        }
    }

    public void Open(int levelUpCount)
    {
        pendingLevelUps += levelUpCount;

        if (pendingLevelUps <= 0)
        {
            return;
        }

        isOpen = true;
        Time.timeScale = 0f;

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
        }

        PickChoices();
        UpdateTexts();
    }

    private void PickChoices()
    {
        currentChoices.Clear();

        List<UpgradeData> pool = new List<UpgradeData>();

        for (int i = 0; i < availableUpgrades.Count; i++)
        {
            if (availableUpgrades[i] != null)
            {
                pool.Add(availableUpgrades[i]);
            }
        }

        int choiceCount = Mathf.Min(3, pool.Count);

        for (int i = 0; i < choiceCount; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            currentChoices.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }
    }

    private void ChooseUpgrade(int choiceIndex)
    {
        if (!CanChoose())
        {
            return;
        }

        if (choiceIndex < 0 || choiceIndex >= currentChoices.Count)
        {
            return;
        }

        ApplyUpgrade(currentChoices[choiceIndex]);
        CompleteOneChoice();
    }

    private bool CanChoose()
    {
        return isOpen && pendingLevelUps > 0;
    }

    private void ApplyUpgrade(UpgradeData upgrade)
    {
        if (upgrade == null)
        {
            return;
        }

        switch (upgrade.UpgradeType)
        {
            case UpgradeType.Damage:
                if (playerWeapon != null)
                {
                    playerWeapon.AddDamage(upgrade.IntValue);
                }
                break;

            case UpgradeType.AttackSpeed:
                if (playerWeapon != null)
                {
                    playerWeapon.ImproveAttackSpeed(upgrade.FloatValue);
                }
                break;

            case UpgradeType.AttackRange:
                if (playerWeapon != null)
                {
                    playerWeapon.ImproveAttackRange(upgrade.FloatValue);
                }
                break;

            case UpgradeType.MaxHealth:
                if (playerHealth != null)
                {
                    playerHealth.AddMaxHealth(upgrade.IntValue, true);
                }
                break;

            case UpgradeType.MoveSpeed:
                if (playerController != null)
                {
                    playerController.ImproveMoveSpeed(upgrade.FloatValue);
                }
                break;

            case UpgradeType.PickupRange:
                if (playerPickupRange != null)
                {
                    playerPickupRange.ImprovePickupRange(upgrade.FloatValue);
                }
                break;

            case UpgradeType.Defense:
                if (playerHealth != null)
                {
                    playerHealth.AddDefense(upgrade.IntValue);
                }
                break;

            case UpgradeType.CriticalChance:
                if (playerWeapon != null)
                {
                    playerWeapon.AddCriticalChance(upgrade.FloatValue);
                }
                break;
        }
    }

    private void CompleteOneChoice()
    {
        pendingLevelUps--;

        if (pendingLevelUps > 0)
        {
            PickChoices();
            UpdateTexts();
            return;
        }

        isOpen = false;
        Time.timeScale = 1f;
        ClosePanelOnly();
    }

    private void ClosePanelOnly()
    {
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }
    }

    private void UpdateTexts()
    {
        if (titleText != null)
        {
            titleText.text = "LEVEL UP";
        }

        if (pendingText != null)
        {
            pendingText.text = "Choices Left: " + pendingLevelUps;
        }

        SetButtonText(damageButtonText, 0);
        SetButtonText(attackSpeedButtonText, 1);
        SetButtonText(attackRangeButtonText, 2);
    }

    private void SetButtonText(TMP_Text buttonText, int choiceIndex)
    {
        if (buttonText == null)
        {
            return;
        }

        if (choiceIndex < 0 || choiceIndex >= currentChoices.Count)
        {
            buttonText.text = "-";
            return;
        }

        UpgradeData upgrade = currentChoices[choiceIndex];

        buttonText.text = upgrade.UpgradeName + "\n" + upgrade.Description;
    }
}