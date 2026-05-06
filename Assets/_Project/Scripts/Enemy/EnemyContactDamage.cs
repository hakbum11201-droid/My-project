using UnityEngine;

public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private int damage = 10;
    [SerializeField] private float damageInterval = 0.8f;

    private float damageTimer;

    public void Initialize(int newDamage)
    {
        damage = Mathf.Max(1, newDamage);
        damageTimer = 0f;
    }

    private void Update()
    {
        if (damageTimer > 0f)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (damageTimer > 0f)
            return;

        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            return;

        Vector2 hitDirection = other.transform.position - transform.position;

        playerHealth.TakeDamage(damage, hitDirection);

        damageTimer = damageInterval;
    }
}