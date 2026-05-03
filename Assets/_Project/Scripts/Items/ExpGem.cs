using UnityEngine;

public class ExpGem : MonoBehaviour
{
    [Header("EXP")]
    [SerializeField] private int expAmount = 5;

    private bool isCollected;

    public void SetExpAmount(int amount)
    {
        expAmount = Mathf.Max(1, amount);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerExp playerExp = other.GetComponent<PlayerExp>();

        if (playerExp == null)
            return;

        TryCollect(playerExp);
    }

    public bool TryCollect(PlayerExp playerExp)
    {
        if (isCollected)
            return false;

        if (playerExp == null)
            return false;

        isCollected = true;
        playerExp.AddExp(expAmount);
        Destroy(gameObject);

        return true;
    }
}