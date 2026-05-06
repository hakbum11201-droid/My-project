using UnityEngine;

public class MagicBoltProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 9f;
    [SerializeField] private float lifeTime = 3f;

    [Header("Hit")]
    [SerializeField] private LayerMask enemyLayer;

    private int damage;
    private Vector2 moveDirection;
    private bool hasHit;
    private float lifeTimer;
    private PlayerMagicBoltAutoAttack owner;

    public void SetOwner(PlayerMagicBoltAutoAttack projectileOwner)
    {
        owner = projectileOwner;
    }

    public void Initialize(Vector2 direction, int newDamage, LayerMask newEnemyLayer)
    {
        moveDirection = direction.normalized;
        damage = Mathf.Max(1, newDamage);
        enemyLayer = newEnemyLayer;
        hasHit = false;
        lifeTimer = lifeTime;
    }

    private void Update()
    {
        if (hasHit)
            return;

        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0f)
        {
            Release();
            return;
        }

        transform.position += (Vector3)(moveDirection * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit)
            return;

        bool isEnemyLayer = ((1 << other.gameObject.layer) & enemyLayer.value) != 0;

        if (!isEnemyLayer)
            return;

        EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();

        if (enemyHealth == null)
            return;

        hasHit = true;

        enemyHealth.TakeDamage(damage, transform.position);

        Release();
    }

    private void OnDisable()
    {
        hasHit = true;
    }

    private void Release()
    {
        hasHit = true;

        if (owner != null)
        {
            owner.ReturnProjectileToPool(this);
            return;
        }

        Destroy(gameObject);
    }
}