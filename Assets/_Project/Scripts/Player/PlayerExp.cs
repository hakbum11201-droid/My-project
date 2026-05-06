using UnityEngine;
using System;

public class PlayerExp : MonoBehaviour
{
    public event Action OnExpChanged;

    [Header("Level")]
    [SerializeField] private int level = 1;
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expToNextLevel = 20;

    [Header("UI")]
    [SerializeField] private LevelUpUI levelUpUI;

    public int Level => level;
    public int CurrentExp => currentExp;
    public int ExpToNextLevel => expToNextLevel;

    public void AddExp(int amount)
    {
        if (amount <= 0)
            return;

        currentExp += amount;

        Debug.Log($"EXP +{amount} / {currentExp}/{expToNextLevel}");

        int levelUpCount = 0;

        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
            levelUpCount++;
        }

        if (levelUpCount > 0 && levelUpUI != null)
        {
            levelUpUI.Open(levelUpCount);
        }

        NotifyExpChanged();
    }

    private void LevelUp()
    {
        level++;
        expToNextLevel = Mathf.RoundToInt(expToNextLevel * 1.35f + 5f);

        Debug.Log($"Level Up! Current Level: {level}");
    }

    private void NotifyExpChanged()
    {
        OnExpChanged?.Invoke();
    }
}