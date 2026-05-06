using System.Collections;
using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerHealth : MonoBehaviour
{
    public event Action OnHealthChanged;
    public event Action OnDied;

    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    [Header("Defense")]
    [SerializeField] private int defense = 0;
    [SerializeField] private int maximumDefense = 20;

    [Header("Hit Reaction")]
    [SerializeField] private float invincibleDuration = 0.6f;
    [SerializeField] private float knockbackForce = 5f;
    [SerializeField] private float knockbackDuration = 0.15f;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float blinkInterval = 0.08f;

    [Header("References")]
    [SerializeField] private PauseManager pauseManager;

    private int currentHealth;
    private bool isDead;
    private bool isInvincible;

    private PlayerController playerController;
    private PlayerMeleeAutoAttack playerWeapon;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool hasLoggedMissingPauseManager;

    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;
    public int Defense => defense;
    public float HealthRate => maxHealth <= 0 ? 0f : (float)currentHealth / maxHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        currentHealth = maxHealth;

        playerController = GetComponent<PlayerController>();
        playerWeapon = GetComponent<PlayerMeleeAutoAttack>();
        if (pauseManager == null)
        {
            // 인스펙터 연결을 우선 사용하고, 누락된 경우에만 1회 자동 탐색합니다.
            pauseManager = FindFirstObjectByType<PauseManager>();
        }
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        NotifyHealthChanged();

        if (pauseManager == null)
        {
            Debug.LogWarning("[PlayerHealth] pauseManager가 비어 있습니다. 가능하면 Inspector에 PauseManager를 연결하세요.", this);
        }
    }

    public void TakeDamage(int damage)
    {
        TakeDamage(damage, Vector2.zero);
    }

    public void TakeDamage(int damage, Vector2 hitDirection)
    {
        if (isDead || isInvincible)
            return;

        if (damage <= 0)
            return;

        int finalDamage = Mathf.Max(1, damage - defense);

        currentHealth -= finalDamage;
        currentHealth = Mathf.Max(currentHealth, 0);
        NotifyHealthChanged();

        Debug.Log($"Player took {finalDamage} damage. Raw: {damage}, Defense: {defense}, HP: {currentHealth}/{maxHealth}");

        if (hitDirection.sqrMagnitude > 0.01f && playerController != null)
        {
            playerController.ApplyKnockback(hitDirection, knockbackForce, knockbackDuration);
        }

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibleRoutine());
    }

    public void AddMaxHealth(int amount, bool healSameAmount)
    {
        if (amount <= 0)
            return;

        maxHealth += amount;

        if (healSameAmount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Min(currentHealth, maxHealth);
        }

        Debug.Log($"Max HP increased. HP: {currentHealth}/{maxHealth}");
        NotifyHealthChanged();
    }

    public void Heal(int amount)
    {
        if (amount <= 0)
            return;

        if (isDead)
            return;

        currentHealth += amount;
        currentHealth = Mathf.Min(currentHealth, maxHealth);

        Debug.Log($"Player healed {amount}. HP: {currentHealth}/{maxHealth}");
        NotifyHealthChanged();
    }

    public void AddDefense(int amount)
    {
        if (amount <= 0)
            return;

        defense += amount;
        defense = Mathf.Min(defense, maximumDefense);

        Debug.Log($"Defense increased. Defense: {defense}");
    }

    private IEnumerator InvincibleRoutine()
    {
        isInvincible = true;

        float timer = 0f;

        while (timer < invincibleDuration)
        {
            if (spriteRenderer != null)
                spriteRenderer.color = hitColor;

            yield return new WaitForSeconds(blinkInterval);

            if (spriteRenderer != null)
                spriteRenderer.color = originalColor;

            yield return new WaitForSeconds(blinkInterval);

            timer += blinkInterval * 2f;
        }

        if (spriteRenderer != null)
            spriteRenderer.color = originalColor;

        isInvincible = false;
    }

    private void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        if (playerController != null)
            playerController.enabled = false;

        if (playerWeapon != null)
            playerWeapon.enabled = false;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.gray;

        RequestPauseOnDeath();
        NotifyHealthChanged();
        OnDied?.Invoke();

        Debug.Log("Game Over. Player died.");
    }

    private void NotifyHealthChanged()
    {
        OnHealthChanged?.Invoke();
    }

    private void RequestPauseOnDeath()
    {
        if (pauseManager != null)
        {
            pauseManager.RequestPause(this);
            return;
        }

        LogMissingPauseManagerWarning();
    }

    private void LogMissingPauseManagerWarning()
    {
        if (hasLoggedMissingPauseManager)
            return;

        hasLoggedMissingPauseManager = true;
        Debug.LogWarning("[PlayerHealth] PauseManager가 없어 사망 시 일시정지 제어를 수행할 수 없습니다. PlayerHealth.pauseManager 연결을 확인하세요.", this);
    }
}