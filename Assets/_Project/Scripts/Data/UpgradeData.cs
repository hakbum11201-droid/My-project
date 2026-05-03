using UnityEngine;

public enum UpgradeType
{
    Damage,
    AttackSpeed,
    AttackRange,
    MaxHealth,
    MoveSpeed,
    PickupRange,
    Defense,
    CriticalChance
}

[CreateAssetMenu(fileName = "Upgrade_", menuName = "_Project/Upgrades/Upgrade Data")]
public class UpgradeData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string upgradeName;
    [SerializeField, TextArea(2, 4)] private string description;
    [SerializeField] private Sprite icon;

    [Header("Effect")]
    [SerializeField] private UpgradeType upgradeType;
    [SerializeField] private int intValue;
    [SerializeField] private float floatValue;

    public string UpgradeName => upgradeName;
    public string Description => description;
    public Sprite Icon => icon;

    public UpgradeType UpgradeType => upgradeType;
    public int IntValue => intValue;
    public float FloatValue => floatValue;
}