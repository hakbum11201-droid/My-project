using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerMeleeAutoAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackInterval = 0.8f;
    [SerializeField] private float attackRange = 1.15f;
    [SerializeField] private float attackRadius = 0.55f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Critical")]
    [SerializeField] private float criticalChance = 0f;
    [SerializeField] private float criticalMultiplier = 2f;
    [SerializeField] private float maximumCriticalChance = 0.5f;

    [Header("Relic Bonus")]
    [SerializeField] private float relicAttackSpeedBonus = 0f;

    [Header("Limit")]
    [SerializeField] private float minimumAttackInterval = 0.2f;
    [SerializeField] private float maximumAttackRange = 3.5f;
    [SerializeField] private float maximumAttackRadius = 2.0f;

    [Header("Visual")]
    [SerializeField] private GameObject attackVisual;
    [SerializeField] private float visualDuration = 0.12f;

    private PlayerController playerController;
    private float attackTimer;
    private Coroutine visualCoroutine;

    public int Damage => damage;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;
    public float AttackRadius => attackRadius;
    public float CriticalChance => criticalChance;
    public float RelicAttackSpeedBonus => relicAttackSpeedBonus;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();

        if (attackVisual != null)
        {
            attackVisual.SetActive(false);
        }
    }

    private void Update()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer <= 0f)
        {
            Attack();
            attackTimer = GetCurrentAttackInterval();
        }
    }

    public void AddDamage(int amount)
    {
        if (amount <= 0)
            return;

        damage += amount;

        Debug.Log($"Weapon damage increased. Damage: {damage}");
    }

    public void ImproveAttackSpeed(float rate)
    {
        if (rate <= 0f)
            return;

        attackInterval *= 1f - rate;
        attackInterval = Mathf.Max(minimumAttackInterval, attackInterval);

        Debug.Log($"Attack speed improved. Interval: {attackInterval}");
    }

    public void ImproveAttackRange(float rate)
    {
        if (rate <= 0f)
            return;

        attackRange *= 1f + rate;
        attackRadius *= 1f + rate;

        attackRange = Mathf.Min(maximumAttackRange, attackRange);
        attackRadius = Mathf.Min(maximumAttackRadius, attackRadius);

        Debug.Log($"Attack range improved. Range: {attackRange}, Radius: {attackRadius}");
    }

    public void AddCriticalChance(float amount)
    {
        if (amount <= 0f)
            return;

        criticalChance += amount;
        criticalChance = Mathf.Min(criticalChance, maximumCriticalChance);

        Debug.Log($"Critical chance increased. Critical Chance: {criticalChance * 100f}%");
    }

    public void SetRelicAttackSpeedBonus(float bonusRate)
    {
        relicAttackSpeedBonus = Mathf.Max(0f, bonusRate);
    }

    private float GetCurrentAttackInterval()
    {
        float finalInterval = attackInterval;

        if (relicAttackSpeedBonus > 0f)
        {
            finalInterval *= 1f - relicAttackSpeedBonus;
        }

        return Mathf.Max(minimumAttackInterval, finalInterval);
    }

    private void Attack()
    {
        if (playerController == null)
            return;

        Vector2 facingDirection = playerController.FacingDirection.normalized;
        Vector2 attackCenter = (Vector2)transform.position + facingDirection * attackRange;

        ShowAttackVisual(facingDirection);

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackCenter,
            attackRadius,
            enemyLayer
        );

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                int finalDamage = CalculateDamage();
                enemyHealth.TakeDamage(finalDamage, transform.position);
            }
        }

        Debug.DrawLine(transform.position, attackCenter, Color.red, 0.2f);
    }

    private int CalculateDamage()
    {
        bool isCritical = Random.value < criticalChance;

        if (!isCritical)
        {
            return damage;
        }

        int criticalDamage = Mathf.RoundToInt(damage * criticalMultiplier);

        Debug.Log($"Critical Hit! Damage: {criticalDamage}");

        return criticalDamage;
    }

    private void ShowAttackVisual(Vector2 direction)
    {
        if (attackVisual == null)
            return;

        attackVisual.transform.localPosition = direction * attackRange;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        attackVisual.transform.localRotation = Quaternion.Euler(0f, 0f, angle);

        if (visualCoroutine != null)
        {
            StopCoroutine(visualCoroutine);
        }

        visualCoroutine = StartCoroutine(AttackVisualRoutine());
    }

    private IEnumerator AttackVisualRoutine()
    {
        attackVisual.SetActive(true);

        yield return new WaitForSeconds(visualDuration);

        attackVisual.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 direction = Vector2.right;

        PlayerController controller = GetComponent<PlayerController>();

        if (controller != null)
        {
            direction = controller.FacingDirection.normalized;
        }

        Vector2 attackCenter = (Vector2)transform.position + direction * attackRange;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCenter, attackRadius);
    }
}