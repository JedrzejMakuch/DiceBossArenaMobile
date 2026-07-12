using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName = "Dice Boss Arena/Skills/Skill Definition")]
public class SkillDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string skillId;
    [SerializeField] private string displayName;
    [SerializeField, TextArea]
    private string description;
    [SerializeField] private Sprite icon;

    [Header("Targeting")]
    [SerializeField] private SkillTargetType targetType;
    [SerializeField] private SkillRangeShape rangeShape = SkillRangeShape.Manhattan;
    [SerializeField, Min(0)] private int minRange;
    [SerializeField, Min(0)] private int maxRange = 1;

    [Header("Turn Costs")]
    [SerializeField, Min(0)] private int actionPointCost = 1;
    [SerializeField, Min(0)] private int movementPointCost;

    [Header("Progression")]
    [SerializeField, Min(1)] private int maxLevel = 1;
    [SerializeField] private List<SkillLevelData> levels = new();

    [Header("Effects")]
    [SerializeField]
    private List<SkillEffectDefinition> effects = new();

    public string SkillId => skillId;
    public string DisplayName => displayName;
    public string Description => description;
    public Sprite Icon => icon;

    public SkillTargetType TargetType => targetType;
    public SkillRangeShape RangeShape => rangeShape;
    public int MinRange => minRange;
    public int MaxRange => maxRange;

    public int ActionPointCost => actionPointCost;
    public int MovementPointCost => movementPointCost;

    public int MaxLevel => maxLevel;

    public IReadOnlyList<SkillEffectDefinition> Effects =>
        effects;

    public SkillLevelData GetLevelData(int level)
    {
        if (levels == null || levels.Count == 0)
        {
            return SkillLevelData.Default;
        }

        int index = Mathf.Clamp(
            level - 1,
            0,
            levels.Count - 1);

        return levels[index];
    }

    private void OnValidate()
    {
        minRange = Mathf.Max(0, minRange);
        maxRange = Mathf.Max(minRange, maxRange);
        maxLevel = Mathf.Max(1, maxLevel);

        if (levels == null)
        {
            levels = new List<SkillLevelData>();
        }

        while (levels.Count < maxLevel)
        {
            levels.Add(SkillLevelData.Default);
        }

        if (levels.Count > maxLevel)
        {
            levels.RemoveRange(
                maxLevel,
                levels.Count - maxLevel);
        }
    }
}