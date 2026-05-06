using UnityEngine;

public class PlayerMagicBoltAutoAttack : MonoBehaviour
{
    [Header("Weapon State")]
    [SerializeField] private bool isUnlocked;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private bool useProjectilePooling = true;
    [SerializeField] private int projectilePoolPrewarmCount = 12;

    [Header("Attack")]
    [SerializeField] private int damage = 8;
    [SerializeField] private float attackInterval = 1.2f;
    [SerializeField] private float attackRange = 6f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Limits")]
    [SerializeField] private float minimumAttackInterval = 0.25f;
    [SerializeField] private float maximumAttackRange = 10f;

    private float attackTimer;
    private readonly System.Collections.Generic.Queue<MagicBoltProjectile> projectilePool = new System.Collections.Generic.Queue<MagicBoltProjectile>();

    public bool IsUnlocked => isUnlocked;
    public int Damage => damage;
    public float AttackInterval => attackInterval;
    public float AttackRange => attackRange;

    private void Awake()
    {
        ValidateRequiredReferences();
        if (useProjectilePooling)
        {
            PrewarmProjectilePool();
        }
        enabled = isUnlocked;
    }

    private void ValidateRequiredReferences()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("[PlayerMagicBoltAutoAttack] projectilePrefab이 비어 있습니다. Inspector에서 매직 볼트 프리팹을 연결하세요.", this);
        }

        if (projectileSpawnPoint == null)
        {
            Debug.LogWarning("[PlayerMagicBoltAutoAttack] projectileSpawnPoint가 비어 있습니다. 기본적으로 Player 위치에서 발사됩니다.", this);
        }
    }

    private void Update()
    {
        if (!isUnlocked)
            return;

        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f)
            return;

        Transform target = FindNearestEnemy();

        if (target == null)
            return;

        Fire(target);

        attackTimer = attackInterval;
    }

    public void Unlock()
    {
        if (isUnlocked)
            return;

        isUnlocked = true;
        enabled = true;
        attackTimer = 0f;

        Debug.Log("Magic Bolt unlocked.");
    }

    public void AddDamage(int amount)
    {
        if (!isUnlocked)
            return;

        if (amount <= 0)
            return;

        damage += amount;

        Debug.Log($"Magic Bolt damage increased. Damage: {damage}");
    }

    public void ImproveAttackSpeed(float bonusRate)
    {
        if (!isUnlocked)
            return;

        if (bonusRate <= 0f)
            return;

        attackInterval *= 1f - bonusRate;
        attackInterval = Mathf.Max(attackInterval, minimumAttackInterval);

        Debug.Log($"Magic Bolt attack speed improved. Interval: {attackInterval}");
    }

    public void ImproveAttackRange(float bonusRate)
    {
        if (!isUnlocked)
            return;

        if (bonusRate <= 0f)
            return;

        attackRange *= 1f + bonusRate;
        attackRange = Mathf.Min(attackRange, maximumAttackRange);

        Debug.Log($"Magic Bolt range improved. Range: {attackRange}");
    }

    private Transform FindNearestEnemy()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            attackRange,
            enemyLayer
        );

        Transform nearestEnemy = null;
        float nearestDistanceSqr = float.MaxValue;

        for (int i = 0; i < hits.Length; i++)
        {
            EnemyHealth enemyHealth = hits[i].GetComponent<EnemyHealth>();

            if (enemyHealth == null)
                continue;

            float distanceSqr = ((Vector2)hits[i].transform.position - (Vector2)transform.position).sqrMagnitude;

            if (distanceSqr < nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearestEnemy = hits[i].transform;
            }
        }

        return nearestEnemy;
    }

    private void Fire(Transform target)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("Magic Bolt projectile prefab is not assigned.");
            return;
        }

        Vector3 spawnPosition = projectileSpawnPoint != null
            ? projectileSpawnPoint.position
            : transform.position;

        Vector2 direction = target.position - spawnPosition;

        if (direction.sqrMagnitude <= 0.01f)
            return;

        MagicBoltProjectile projectile = GetOrCreateProjectile();

        if (projectile == null)
        {
            Debug.LogWarning("MagicBoltProjectile component is missing on projectile prefab.");
            return;
        }

        projectile.transform.position = spawnPosition;
        projectile.transform.rotation = Quaternion.identity;
        projectile.gameObject.SetActive(true);
        projectile.Initialize(direction, damage, enemyLayer);
    }

    public void ReturnProjectileToPool(MagicBoltProjectile projectile)
    {
        if (projectile == null)
            return;

        if (!useProjectilePooling)
        {
            Destroy(projectile.gameObject);
            return;
        }

        projectile.gameObject.SetActive(false);
        projectilePool.Enqueue(projectile);
    }

    private void PrewarmProjectilePool()
    {
        if (projectilePrefab == null)
            return;

        for (int i = 0; i < projectilePoolPrewarmCount; i++)
        {
            MagicBoltProjectile projectile = CreateProjectileInstance();

            if (projectile == null)
                continue;

            projectile.gameObject.SetActive(false);
            projectilePool.Enqueue(projectile);
        }
    }

    private MagicBoltProjectile GetOrCreateProjectile()
    {
        if (!useProjectilePooling)
        {
            return CreateProjectileInstance();
        }

        if (projectilePool.Count > 0)
        {
            return projectilePool.Dequeue();
        }

        return CreateProjectileInstance();
    }

    private MagicBoltProjectile CreateProjectileInstance()
    {
        if (projectilePrefab == null)
            return null;

        GameObject projectileObject = Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
        MagicBoltProjectile projectile = projectileObject.GetComponent<MagicBoltProjectile>();

        if (projectile == null)
        {
            Destroy(projectileObject);
            return null;
        }

        projectile.SetOwner(this);
        return projectile;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}