using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PauseManager pauseManager;
    [SerializeField] private PlayerMeleeAutoAttack playerWeapon;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PlayerPickupRange playerPickupRange;
    [SerializeField] private PlayerMagicBoltAutoAttack playerMagicBolt;

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
        ValidateRequiredReferences();
        RegisterButtonEvents();
        ClosePanelOnly();
    }

    private void ValidateRequiredReferences()
    {
        if (levelUpPanel == null)
        {
            Debug.LogWarning("[LevelUpUI] levelUpPanel이 비어 있습니다. Hierarchy의 LevelUpPanel 연결을 확인하세요.", this);
        }

        if (damageButton == null || attackSpeedButton == null || attackRangeButton == null)
        {
            Debug.LogWarning("[LevelUpUI] 강화 선택 버튼 참조가 일부 비어 있습니다. Damage/AttackSpeed/AttackRange 버튼 연결을 확인하세요.", this);
        }

        if (damageButtonText == null || attackSpeedButtonText == null || attackRangeButtonText == null)
        {
            Debug.LogWarning("[LevelUpUI] 버튼 TMP_Text 참조가 일부 비어 있습니다. damage/attackSpeed/attackRangeButtonText 필드 연결을 확인하세요.", this);
        }

        if (playerHealth == null)
        {
            Debug.LogWarning("[LevelUpUI] playerHealth가 비어 있습니다. Player 오브젝트의 PlayerHealth를 연결하세요.", this);
        }

        if (pauseManager == null)
        {
            pauseManager = FindFirstObjectByType<PauseManager>();
        }

        if (pauseManager == null)
        {
            Debug.LogWarning("[LevelUpUI] pauseManager가 비어 있습니다. PauseManager 연결을 권장합니다. (없으면 Time.timeScale 폴백 사용)", this);
        }
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

        if (playerMagicBolt == null && playerHealth != null)
        {
            playerMagicBolt = playerHealth.GetComponent<PlayerMagicBoltAutoAttack>();
        }

        if (playerMagicBolt == null && playerController != null)
        {
            playerMagicBolt = playerController.GetComponent<PlayerMagicBoltAutoAttack>();
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
        if (levelUpPanel == null)
        {
            Debug.LogWarning("[LevelUpUI] levelUpPanel이 없어 레벨업 UI를 열 수 없습니다.", this);
            return;
        }

        pendingLevelUps += levelUpCount;

        if (pendingLevelUps <= 0)
        {
            return;
        }

        isOpen = true;
        RequestPause();

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
            UpgradeData upgrade = availableUpgrades[i];

            if (upgrade == null)
                continue;

            if (!CanAppearInChoices(upgrade))
                continue;

            pool.Add(upgrade);
        }

        int choiceCount = Mathf.Min(3, pool.Count);

        for (int i = 0; i < choiceCount; i++)
        {
            int randomIndex = Random.Range(0, pool.Count);
            currentChoices.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }
    }

    private bool CanAppearInChoices(UpgradeData upgrade)
    {
        if (upgrade == null)
            return false;

        if (upgrade.UpgradeType != UpgradeType.WeaponUnlock)
            return true;

        if (upgrade.WeaponId == "magic_bolt")
        {
            if (playerMagicBolt == null)
                return true;

            return !playerMagicBolt.IsUnlocked;
        }

        return true;
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

                if (playerMagicBolt != null)
                {
                    playerMagicBolt.AddDamage(upgrade.IntValue);
                }
                break;

            case UpgradeType.AttackSpeed:
                if (playerWeapon != null)
                {
                    playerWeapon.ImproveAttackSpeed(upgrade.FloatValue);
                }

                if (playerMagicBolt != null)
                {
                    playerMagicBolt.ImproveAttackSpeed(upgrade.FloatValue);
                }
                break;

            case UpgradeType.AttackRange:
                if (playerWeapon != null)
                {
                    playerWeapon.ImproveAttackRange(upgrade.FloatValue);
                }

                if (playerMagicBolt != null)
                {
                    playerMagicBolt.ImproveAttackRange(upgrade.FloatValue);
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

            case UpgradeType.WeaponUnlock:
                UnlockWeapon(upgrade.WeaponId);
                break;
        }
    }

    private void UnlockWeapon(string weaponId)
    {
        if (string.IsNullOrEmpty(weaponId))
        {
            Debug.LogWarning("Weapon unlock failed. WeaponId is empty.");
            return;
        }

        switch (weaponId)
        {
            case "magic_bolt":
                if (playerMagicBolt != null)
                {
                    playerMagicBolt.Unlock();
                }
                else
                {
                    Debug.LogWarning("Magic Bolt unlock failed. PlayerMagicBoltAutoAttack is not found on Player.");
                }
                break;

            default:
                Debug.LogWarning($"Unknown weapon id: {weaponId}");
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
        ReleasePause();
        ClosePanelOnly();
    }

    private void OnDisable()
    {
        if (isOpen)
        {
            ReleasePause();
            isOpen = false;
        }
    }

    private void RequestPause()
    {
        if (pauseManager != null)
        {
            pauseManager.RequestPause(this);
            return;
        }

        Time.timeScale = 0f;
    }

    private void ReleasePause()
    {
        if (pauseManager != null)
        {
            pauseManager.ReleasePause(this);
            return;
        }

        Time.timeScale = 1f;
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