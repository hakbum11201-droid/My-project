using UnityEngine;
using System;

public class WaveManager : MonoBehaviour
{
    public event Action OnWaveChanged;

    [Header("References")]
    [SerializeField] private GameTimer gameTimer;

    [Header("Wave")]
    [SerializeField] private int currentWave = 1;
    [SerializeField] private float waveDuration = 30f;

    [Header("Difficulty")]
    [SerializeField] private int baseEnemyHealth = 30;
    [SerializeField] private int healthPerWave = 5;
    [SerializeField] private int baseEnemyDamage = 8;
    [SerializeField] private int damagePerWave = 1;
    [SerializeField] private int baseExpReward = 5;

    [Header("Spawn Difficulty")]
    [SerializeField] private float spawnIntervalDecreasePerWave = 0.05f;
    [SerializeField] private float minSpawnInterval = 0.5f;
    [SerializeField] private int maxEnemiesIncreasePerWave = 2;
    [SerializeField] private int maxEnemiesCap = 50;

    [Header("Timed Mid Boss")]
    [SerializeField] private float firstMidBossSpawnTime = 300f;
    [SerializeField] private float midBossHealthMultiplier = 15f;
    [SerializeField] private float midBossDamageMultiplier = 2f;
    [SerializeField] private float midBossExpMultiplier = 4f;

    [Header("Timed Strong Boss")]
    [SerializeField] private float strongBossSpawnTime = 600f;
    [SerializeField] private float strongBossHealthMultiplier = 18f;
    [SerializeField] private float strongBossDamageMultiplier = 2.2f;
    [SerializeField] private float strongBossExpMultiplier = 8f;

    private float waveTimer;
    private bool firstMidBossSpawned;
    private bool strongBossSpawned;

    public int CurrentWave => currentWave;
    public float WaveTimer => waveTimer;
    public float WaveDuration => waveDuration;
    public float FirstMidBossSpawnTime => firstMidBossSpawnTime;
    public bool FirstMidBossSpawned => firstMidBossSpawned;
    public float StrongBossSpawnTime => strongBossSpawnTime;
    public bool StrongBossSpawned => strongBossSpawned;

    private void Awake()
    {
        if (gameTimer == null)
        {
            gameTimer = FindFirstObjectByType<GameTimer>();
        }

        waveTimer = waveDuration;
        OnWaveChanged?.Invoke();
    }

    private void Update()
    {
        waveTimer -= Time.deltaTime;

        if (waveTimer <= 0f)
        {
            AdvanceWave();
        }
    }

    private void AdvanceWave()
    {
        currentWave++;
        waveTimer = waveDuration;
        OnWaveChanged?.Invoke();

        Debug.Log($"Wave {currentWave} started.");
    }

    public bool ShouldSpawnFirstMidBoss()
    {
        if (firstMidBossSpawned)
            return false;

        if (gameTimer == null)
            return false;

        return gameTimer.GameplayTime >= firstMidBossSpawnTime;
    }

    public void MarkFirstMidBossSpawned()
    {
        firstMidBossSpawned = true;
        OnWaveChanged?.Invoke();
    }

    public bool ShouldSpawnStrongBoss()
    {
        if (strongBossSpawned)
            return false;

        if (gameTimer == null)
            return false;

        return gameTimer.GameplayTime >= strongBossSpawnTime;
    }

    public void MarkStrongBossSpawned()
    {
        strongBossSpawned = true;
        OnWaveChanged?.Invoke();
    }

    // 기존 HUDCanvasUI / HUDDebugUI 호환용
    public bool IsMidBossWave()
    {
        return ShouldSpawnFirstMidBoss();
    }

    public int GetEnemyHealth(bool isMidBoss, bool isStrongBoss = false)
    {
        int health = baseEnemyHealth + (currentWave - 1) * healthPerWave;

        if (isStrongBoss)
            health = Mathf.RoundToInt(health * strongBossHealthMultiplier);
        else if (isMidBoss)
            health = Mathf.RoundToInt(health * midBossHealthMultiplier);

        return health;
    }

    public int GetEnemyDamage(bool isMidBoss, bool isStrongBoss = false)
    {
        int damage = baseEnemyDamage + (currentWave - 1) * damagePerWave;

        if (isStrongBoss)
            damage = Mathf.RoundToInt(damage * strongBossDamageMultiplier);
        else if (isMidBoss)
            damage = Mathf.RoundToInt(damage * midBossDamageMultiplier);

        return damage;
    }

    public int GetExpReward(bool isMidBoss, bool isStrongBoss = false)
    {
        int exp = baseExpReward + currentWave;

        if (isStrongBoss)
            exp = Mathf.RoundToInt(exp * strongBossExpMultiplier);
        else if (isMidBoss)
            exp = Mathf.RoundToInt(exp * midBossExpMultiplier);

        return exp;
    }

    public float GetSpawnInterval(float baseInterval)
    {
        float interval = baseInterval - ((currentWave - 1) * spawnIntervalDecreasePerWave);
        return Mathf.Max(interval, minSpawnInterval);
    }

    public int GetMaxEnemies(int baseMax)
    {
        int max = baseMax + ((currentWave - 1) * maxEnemiesIncreasePerWave);
        return Mathf.Min(max, maxEnemiesCap);
    }
}