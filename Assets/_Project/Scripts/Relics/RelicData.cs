using UnityEngine;

public enum RelicType
{
    BerserkerFang,
    IronCharm,
    BloodSigil,
    HunterEye,
    GraveMagnet
}

[CreateAssetMenu(fileName = "Relic_", menuName = "_Project/Relics/Relic Data")]
public class RelicData : ScriptableObject
{
    [Header("Basic Info")]
    [SerializeField] private string relicName;
    [SerializeField, TextArea(2, 5)] private string description;
    [SerializeField] private Sprite icon;

    [Header("Effect")]
    [SerializeField] private RelicType relicType;
    [SerializeField] private int intValue;
    [SerializeField] private float floatValue;
    [SerializeField] private float secondaryFloatValue;

    [Header("Stack")]
    [SerializeField] private bool canStack;
    [SerializeField] private int maxStack = 1;

    public string RelicName => relicName;
    public string Description => description;
    public Sprite Icon => icon;

    public RelicType RelicType => relicType;
    public int IntValue => intValue;
    public float FloatValue => floatValue;
    public float SecondaryFloatValue => secondaryFloatValue;

    public bool CanStack => canStack;
    public int MaxStack => maxStack;
}