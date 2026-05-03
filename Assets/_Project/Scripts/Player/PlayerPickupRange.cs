using UnityEngine;

[RequireComponent(typeof(PlayerExp))]
public class PlayerPickupRange : MonoBehaviour
{
    [Header("Pickup")]
    [SerializeField] private float pickupRadius = 1.5f;
    [SerializeField] private float maximumPickupRadius = 6f;
    [SerializeField] private float scanInterval = 0.1f;
    [SerializeField] private LayerMask targetLayers = ~0;

    private PlayerExp playerExp;
    private float scanTimer;

    public float PickupRadius => pickupRadius;

    private void Awake()
    {
        playerExp = GetComponent<PlayerExp>();
    }

    private void Update()
    {
        scanTimer -= Time.deltaTime;

        if (scanTimer > 0f)
            return;

        scanTimer = scanInterval;
        ScanForExpGems();
    }

    private void ScanForExpGems()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRadius, targetLayers);

        for (int i = 0; i < hits.Length; i++)
        {
            ExpGem expGem = hits[i].GetComponent<ExpGem>();

            if (expGem == null)
                continue;

            expGem.TryCollect(playerExp);
        }
    }

    public void ImprovePickupRange(float bonusRate)
    {
        if (bonusRate <= 0f)
            return;

        pickupRadius *= 1f + bonusRate;
        pickupRadius = Mathf.Min(pickupRadius, maximumPickupRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRadius);
    }
}