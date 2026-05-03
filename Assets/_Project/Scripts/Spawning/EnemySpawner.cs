using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Serializable]
    public class EnemySpawnEntry
    {
        public bool enabled = true;
        public string displayName;
        public GameObject enemyPrefab;

        [Min(0f)]
        public float spawnWeight = 1f;
    }

    [Header("References")]
    [SerializeField] private Transform playerTarget;
    [SerializeField] private WaveManager waveManager;

    [Header("Enemy Spawn Entries")]
    [SerializeField] private List<EnemySpawnEntry> enemySpawnEntries = new List<EnemySpawnEntry>();

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float spawnRadius = 8f;
    [SerializeField] private int maxEnemies = 15;

    private readonly List<GameObject> spawnedEnemies = new List<GameObject>();

    private float spawnTimer;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        if (playerTarget != null)
        {
            playerHealth = playerTarget.GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (playerTarget == null || waveManager == null)
            return;

        if (!HasValidEnemyPrefab())
            return;

        if (playerHealth != null && playerHealth.IsDead)
            return;

        RemoveDeadEnemies();

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            TrySpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    private bool HasValidEnemyPrefab()
    {
        for (int i = 0; i < enemySpawnEntries.Count; i++)
        {
            EnemySpawnEntry entry = enemySpawnEntries[i];

            if (entry == null)
                continue;

            if (!entry.enabled)
                continue;

            if (entry.enemyPrefab == null)
                continue;

            if (entry.spawnWeight <= 0f)
                continue;

            return true;
        }

        return false;
    }

    private void TrySpawnEnemy()
    {
        if (waveManager.ShouldSpawnFirstMidBoss())
        {
            SpawnEnemy(true);
            waveManager.MarkFirstMidBossSpawned();
            return;
        }

        if (spawnedEnemies.Count >= maxEnemies)
            return;

        SpawnEnemy(false);
    }

    private void SpawnEnemy(bool isMidBoss)
    {
        GameObject selectedPrefab = GetRandomEnemyPrefab();

        if (selectedPrefab == null)
            return;

        Vector2 spawnPosition = GetSpawnPositionAroundPlayer();

        GameObject enemy = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);

        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        if (enemyMovement != null)
        {
            enemyMovement.SetTarget(playerTarget);
        }

        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

        if (enemyHealth != null)
        {
            enemyHealth.Initialize(
                waveManager.GetEnemyHealth(isMidBoss),
                waveManager.GetExpReward(isMidBoss),
                isMidBoss
            );
        }

        EnemyContactDamage contactDamage = enemy.GetComponent<EnemyContactDamage>();

        if (contactDamage != null)
        {
            contactDamage.Initialize(waveManager.GetEnemyDamage(isMidBoss));
        }

        spawnedEnemies.Add(enemy);

        if (isMidBoss)
        {
            Debug.Log($"Mid Boss spawned at {waveManager.FirstMidBossSpawnTime} seconds.");
        }
    }

    private GameObject GetRandomEnemyPrefab()
    {
        float totalWeight = GetTotalSpawnWeight();

        if (totalWeight <= 0f)
            return null;

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        for (int i = 0; i < enemySpawnEntries.Count; i++)
        {
            EnemySpawnEntry entry = enemySpawnEntries[i];

            if (!IsValidEntry(entry))
                continue;

            currentWeight += entry.spawnWeight;

            if (randomValue <= currentWeight)
                return entry.enemyPrefab;
        }

        return null;
    }

    private float GetTotalSpawnWeight()
    {
        float totalWeight = 0f;

        for (int i = 0; i < enemySpawnEntries.Count; i++)
        {
            EnemySpawnEntry entry = enemySpawnEntries[i];

            if (!IsValidEntry(entry))
                continue;

            totalWeight += entry.spawnWeight;
        }

        return totalWeight;
    }

    private bool IsValidEntry(EnemySpawnEntry entry)
    {
        if (entry == null)
            return false;

        if (!entry.enabled)
            return false;

        if (entry.enemyPrefab == null)
            return false;

        if (entry.spawnWeight <= 0f)
            return false;

        return true;
    }

    private Vector2 GetSpawnPositionAroundPlayer()
    {
        Vector2 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;

        if (randomDirection.sqrMagnitude <= 0.01f)
        {
            randomDirection = Vector2.right;
        }

        Vector2 playerPosition = playerTarget.position;

        return playerPosition + randomDirection * spawnRadius;
    }

    private void RemoveDeadEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }
    }
}