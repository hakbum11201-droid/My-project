using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyMovement : MonoBehaviour
{
    private static readonly List<EnemyMovement> ActiveEnemies = new List<EnemyMovement>();

    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Movement")]
    [SerializeField] private float baseMoveSpeed = 2.4f;
    [SerializeField] private float stopDistance = 0.45f;

    [Header("Swarm Target Offset")]
    [SerializeField] private float targetOffsetMinRadius = 0.25f;
    [SerializeField] private float targetOffsetMaxRadius = 1.05f;
    [SerializeField] private float offsetChangeInterval = 2.5f;

    [Header("Separation")]
    [SerializeField] private float separationRadius = 0.85f;
    [SerializeField] private float separationWeight = 1.15f;
    [SerializeField] private float maxSeparationForce = 1.4f;

    [Header("Near Player Behavior")]
    [SerializeField] private float nearPlayerDistance = 1.25f;
    [SerializeField] private float nearPlayerChaseWeight = 0.55f;

    private Rigidbody2D rb;

    private Vector2 personalTargetOffset;
    private float moveSpeed;
    private float offsetTimer;

    private Vector2 knockbackVelocity;
    private float knockbackTimer;

    private void OnEnable()
    {
        if (!ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Add(this);
        }

        knockbackVelocity = Vector2.zero;
        knockbackTimer = 0f;
        PickNewTargetOffset();
        offsetTimer = Random.Range(0f, offsetChangeInterval);
    }

    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        moveSpeed = baseMoveSpeed * Random.Range(0.9f, 1.12f);
        PickNewTargetOffset();
        offsetTimer = Random.Range(0f, offsetChangeInterval);
    }

    private void FixedUpdate()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.fixedDeltaTime;
            rb.linearVelocity = knockbackVelocity;
            return;
        }

        UpdateTargetOffsetTimer();
        MoveToTargetArea();
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ApplyKnockback(Vector2 direction, float force, float duration)
    {
        if (direction.sqrMagnitude <= 0.01f)
            return;

        knockbackVelocity = direction.normalized * force;
        knockbackTimer = duration;
    }

    private void UpdateTargetOffsetTimer()
    {
        offsetTimer -= Time.fixedDeltaTime;

        if (offsetTimer <= 0f)
        {
            PickNewTargetOffset();
            offsetTimer = offsetChangeInterval;
        }
    }

    private void PickNewTargetOffset()
    {
        Vector2 direction = Random.insideUnitCircle;

        if (direction.sqrMagnitude <= 0.01f)
        {
            direction = Vector2.right;
        }

        direction.Normalize();

        float radius = Random.Range(targetOffsetMinRadius, targetOffsetMaxRadius);
        personalTargetOffset = direction * radius;
    }

    private void MoveToTargetArea()
    {
        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 currentPosition = rb.position;

        Vector2 desiredTargetPosition = (Vector2)target.position + personalTargetOffset;
        Vector2 toTarget = desiredTargetPosition - currentPosition;

        float distanceToRealPlayer = Vector2.Distance(currentPosition, target.position);
        float distanceToTargetArea = toTarget.magnitude;

        Vector2 chaseDirection = distanceToTargetArea > 0.01f
            ? toTarget.normalized
            : Vector2.zero;

        Vector2 separationDirection = GetSeparationDirection();

        float chaseWeight = distanceToRealPlayer <= nearPlayerDistance
            ? nearPlayerChaseWeight
            : 1f;

        Vector2 finalDirection =
            chaseDirection * chaseWeight +
            separationDirection * separationWeight;

        if (distanceToTargetArea <= stopDistance && separationDirection.sqrMagnitude <= 0.01f)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (finalDirection.sqrMagnitude <= 0.01f)
        {
            finalDirection = chaseDirection;
        }

        rb.linearVelocity = finalDirection.normalized * moveSpeed;
    }

    private Vector2 GetSeparationDirection()
    {
        Vector2 separation = Vector2.zero;

        for (int i = 0; i < ActiveEnemies.Count; i++)
        {
            EnemyMovement otherEnemy = ActiveEnemies[i];

            if (otherEnemy == null || otherEnemy == this)
                continue;

            Vector2 away = (Vector2)(transform.position - otherEnemy.transform.position);
            float distanceSqr = away.sqrMagnitude;

            if (distanceSqr <= 0.0001f)
            {
                away = Random.insideUnitCircle.normalized;
                distanceSqr = 0.0001f;
            }

            float radiusSqr = separationRadius * separationRadius;

            if (distanceSqr > radiusSqr)
                continue;

            float distance = Mathf.Sqrt(distanceSqr);
            float strength = 1f - Mathf.Clamp01(distance / separationRadius);

            separation += away.normalized * strength;
        }

        if (separation.magnitude > maxSeparationForce)
        {
            separation = separation.normalized * maxSeparationForce;
        }

        return separation;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, separationRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetOffsetMaxRadius);
    }
}