using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 30;

    [Header("Reward")]
    [SerializeField] private GameObject expGemPrefab;
    [SerializeField] private int expReward = 5;

    [Header("Small Heal Drop")]
    [SerializeField] private GameObject smallHealPrefab;
    [SerializeField, Range(0f, 1f)] private float smallHealDropChance = 0.07f;
    [SerializeField] private bool midBossAlwaysDropSmallHeal = true;
    [SerializeField] private float smallHealDropOffsetMin = 0.35f;
    [SerializeField] private float smallHealDropOffsetMax = 0.75f;
    [SerializeField] private float expGemDropOffsetRadius = 0.15f;
    [SerializeField] private float minDistanceBetweenDrops = 0.3f;

    [Header("Boss Visual")]
    [SerializeField] private float normalScale = 0.8f;
    [SerializeField] private float midBossScale = 1.35f;

    [Tooltip("체크하면 중간보스에만 색상 강조를 적용합니다.")]
    [SerializeField] private bool useMidBossTint = true;

    [SerializeField] private Color midBossTintColor = new Color(1f, 0.65f, 0.12f, 1f);

    [Header("Hit Reaction")]
    [SerializeField] private Color hitFlashColor = new Color(1f, 0.35f, 0.25f, 1f);
    [SerializeField] private float hitFlashDuration = 0.12f;
    [SerializeField] private float hitScaleMultiplier = 1.12f;
    [SerializeField] private float knockbackForce = 3.5f;
    [SerializeField] private float knockbackDuration = 0.08f;

    private int currentHealth;
    private bool isDead;
    private bool isMidBoss;

    private SpriteRenderer spriteRenderer;
    private EnemyMovement enemyMovement;
    private Color originalColor;
    private Vector3 baseScale;
    private Coroutine hitFlashCoroutine;
    private EnemySpawner ownerSpawner;

    public bool IsMidBoss => isMidBoss;

    private void Awake()
    {
        currentHealth = maxHealth;

        spriteRenderer = GetComponent<SpriteRenderer>();
        enemyMovement = GetComponent<EnemyMovement>();

        baseScale = transform.localScale;

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    public void Initialize(int newMaxHealth, int newExpReward, bool isBoss, EnemySpawner spawner = null)
    {
        ownerSpawner = spawner;
        isMidBoss = isBoss;
        isDead = false;

        maxHealth = Mathf.Max(1, newMaxHealth);
        currentHealth = maxHealth;
        expReward = Mathf.Max(1, newExpReward);

        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
            hitFlashCoroutine = null;
        }

        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (isMidBoss)
        {
            gameObject.name = "MidBoss_Enemy";
            transform.localScale = Vector3.one * midBossScale;
            baseScale = transform.localScale;

            if (spriteRenderer != null)
            {
                if (useMidBossTint)
                {
                    spriteRenderer.color = midBossTintColor;
                    originalColor = midBossTintColor;
                }
                else
                {
                    spriteRenderer.color = Color.white;
                    originalColor = Color.white;
                }
            }

            return;
        }

        gameObject.name = "Enemy";
        transform.localScale = Vector3.one * normalScale;
        baseScale = transform.localScale;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.white;
            originalColor = Color.white;
        }
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, transform.position);
    }

    public void TakeDamage(int damage, Vector2 attackerPosition)
    {
        if (isDead)
            return;

        if (damage <= 0)
            return;

        currentHealth -= damage;

        Vector2 knockbackDirection = (Vector2)transform.position - attackerPosition;

        if (enemyMovement != null)
        {
            float finalKnockbackForce = isMidBoss ? knockbackForce * 0.45f : knockbackForce;
            enemyMovement.ApplyKnockback(knockbackDirection, finalKnockbackForce, knockbackDuration);
        }

        PlayHitFlash();

        Debug.Log($"{gameObject.name} took {damage} damage. HP: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayHitFlash()
    {
        if (hitFlashCoroutine != null)
        {
            StopCoroutine(hitFlashCoroutine);
        }

        hitFlashCoroutine = StartCoroutine(HitFlashRoutine());
    }

    private IEnumerator HitFlashRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitFlashColor;
        }

        transform.localScale = baseScale * hitScaleMultiplier;

        yield return new WaitForSeconds(hitFlashDuration);

        if (!isDead)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            transform.localScale = baseScale;
        }

        hitFlashCoroutine = null;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        NotifyEnemyKilled();

        Vector3 expGemDropPosition = GetExpGemDropPosition();
        DropExpGem(expGemDropPosition);
        DropSmallHeal(expGemDropPosition);

        if (isMidBoss)
        {
            OpenRelicSelectUI();
        }

        if (ownerSpawner != null)
        {
            ownerSpawner.DespawnEnemy(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void NotifyEnemyKilled()
    {
        PlayerRelicEffects relicEffects = FindFirstObjectByType<PlayerRelicEffects>();

        if (relicEffects == null)
            return;

        relicEffects.OnEnemyKilled();
    }

    private void DropExpGem(Vector3 dropPosition)
    {
        if (expGemPrefab == null)
            return;

        GameObject expGem = Instantiate(expGemPrefab, dropPosition, Quaternion.identity);

        ExpGem gem = expGem.GetComponent<ExpGem>();

        if (gem != null)
        {
            gem.SetExpAmount(expReward);
        }
    }

    private void DropSmallHeal(Vector3 expGemDropPosition)
    {
        if (smallHealPrefab == null)
            return;

        if (isMidBoss && midBossAlwaysDropSmallHeal)
        {
            Instantiate(smallHealPrefab, GetSmallHealDropPosition(expGemDropPosition), Quaternion.identity);
            return;
        }

        if (Random.value > smallHealDropChance)
            return;

        Instantiate(smallHealPrefab, GetSmallHealDropPosition(expGemDropPosition), Quaternion.identity);
    }

    private Vector3 GetExpGemDropPosition()
    {
        Vector2 randomOffset = Random.insideUnitCircle * expGemDropOffsetRadius;
        return transform.position + (Vector3)randomOffset;
    }

    private Vector3 GetSmallHealDropPosition(Vector3 expGemDropPosition)
    {
        Vector3 smallHealPosition = transform.position;
        int maxRetryCount = 5;

        for (int i = 0; i < maxRetryCount; i++)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;

            if (randomDirection.sqrMagnitude <= 0.01f)
            {
                randomDirection = Vector2.right;
            }

            float distance = Random.Range(smallHealDropOffsetMin, smallHealDropOffsetMax);
            smallHealPosition = transform.position + (Vector3)(randomDirection * distance);

            if ((smallHealPosition - expGemDropPosition).sqrMagnitude >= minDistanceBetweenDrops * minDistanceBetweenDrops)
            {
                break;
            }
        }

        return smallHealPosition;
    }

    private void OpenRelicSelectUI()
    {
        RelicSelectUI relicSelectUI = FindFirstObjectByType<RelicSelectUI>();

        if (relicSelectUI == null)
        {
            Debug.LogWarning("RelicSelectUI not found in scene.");
            return;
        }

        relicSelectUI.Open();
    }
}