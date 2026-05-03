using UnityEngine;

[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerMeleeAutoAttack))]
public class PlayerRelicEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerMeleeAutoAttack playerWeapon;

    [Header("Berserker Fang")]
    [SerializeField] private bool hasBerserkerFang;
    [SerializeField] private float berserkerAttackSpeedBonus = 0.25f;
    [SerializeField] private float berserkerHpThreshold = 0.4f;

    [Header("Blood Sigil")]
    [SerializeField] private bool hasBloodSigil;
    [SerializeField, Range(0f, 1f)] private float bloodSigilHealChance = 0.05f;
    [SerializeField] private int bloodSigilHealAmount = 1;

    private void Awake()
    {
        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }

        if (playerWeapon == null)
        {
            playerWeapon = GetComponent<PlayerMeleeAutoAttack>();
        }
    }

    private void Update()
    {
        UpdateBerserkerFang();
    }

    public void ActivateBerserkerFang(float attackSpeedBonus, float hpThreshold)
    {
        hasBerserkerFang = true;
        berserkerAttackSpeedBonus = Mathf.Max(0f, attackSpeedBonus);
        berserkerHpThreshold = Mathf.Clamp01(hpThreshold);

        Debug.Log($"Berserker Fang activated. Bonus: {berserkerAttackSpeedBonus}, HP Threshold: {berserkerHpThreshold}");
    }

    public void ActivateBloodSigil(float healChance, int healAmount)
    {
        hasBloodSigil = true;
        bloodSigilHealChance = Mathf.Clamp01(healChance);
        bloodSigilHealAmount = Mathf.Max(1, healAmount);

        Debug.Log($"Blood Sigil activated. Chance: {bloodSigilHealChance}, Heal: {bloodSigilHealAmount}");
    }

    public void OnEnemyKilled()
    {
        if (!hasBloodSigil)
            return;

        if (playerHealth == null)
            return;

        if (Random.value > bloodSigilHealChance)
            return;

        playerHealth.Heal(bloodSigilHealAmount);

        Debug.Log("Blood Sigil healed player.");
    }

    private void UpdateBerserkerFang()
    {
        if (playerWeapon == null)
            return;

        if (!hasBerserkerFang || playerHealth == null)
        {
            playerWeapon.SetRelicAttackSpeedBonus(0f);
            return;
        }

        if (playerHealth.HealthRate <= berserkerHpThreshold)
        {
            playerWeapon.SetRelicAttackSpeedBonus(berserkerAttackSpeedBonus);
        }
        else
        {
            playerWeapon.SetRelicAttackSpeedBonus(0f);
        }
    }
}