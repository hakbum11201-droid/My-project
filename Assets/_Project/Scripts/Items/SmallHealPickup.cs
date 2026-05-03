using UnityEngine;

public class SmallHealPickup : MonoBehaviour
{
    [Header("Heal")]
    [SerializeField] private int healAmount = 10;

    private bool isCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

        if (playerHealth == null)
            return;

        TryCollect(playerHealth);
    }

    public bool TryCollect(PlayerHealth playerHealth)
    {
        if (isCollected)
            return false;

        if (playerHealth == null)
            return false;

        isCollected = true;

        playerHealth.Heal(healAmount);

        Destroy(gameObject);

        return true;
    }
}