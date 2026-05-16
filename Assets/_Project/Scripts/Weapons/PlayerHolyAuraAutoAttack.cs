using UnityEngine;

public class PlayerHolyAuraAutoAttack : MonoBehaviour
{
    [Header("Weapon State")]
    [SerializeField] private bool isUnlocked;

    [Header("Attack")]
    [SerializeField] private int damage = 4;
    [SerializeField] private float attackInterval = 1.0f;
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Limits")]
    [SerializeField] private float minimumAttackInterval = 0.25f;
    [SerializeField] private float maximumAttackRange = 5.5f;

    private float attackTimer;

    public bool IsUnlocked => isUnlocked;
    public int Damage => damage;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;

    private void Awake()
    {
        enabled = isUnlocked;
    }

    private void Update()
    {
        if (!isUnlocked)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
            return;

        PerformAuraAttack();

        attackTimer = attackInterval;
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        isUnlocked = true;
        enabled = true;
        attackTimer = 0f;

        Debug.Log("Holy Aura unlocked.");
    }

    public void AddDamage(int amount)
    {
        if (!isUnlocked)
            return;

        if (amount <= 0)
            return;

        damage += amount;

        Debug.Log($"Holy Aura damage increased. Damage: {damage}");
    }

    public void ImproveAttackSpeed(float bonusRate)
    {
        if (!isUnlocked)
            return;

        if (bonusRate <= 0f)
            return;

        attackInterval *= 1f - bonusRate;
        attackInterval = Mathf.Max(attackInterval, minimumAttackInterval);

        Debug.Log($"Holy Aura attack speed improved. Interval: {attackInterval}");
    }

    public void ImproveAttackRange(float bonusRate)
    {
        if (!isUnlocked)
            return;

        if (bonusRate <= 0f)
            return;

        attackRange *= 1f + bonusRate;
        attackRange = Mathf.Min(attackRange, maximumAttackRange);

        Debug.Log($"Holy Aura range improved. Range: {attackRange}");
    }

    private void PerformAuraAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemyHealth = hits[i].GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage, transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
