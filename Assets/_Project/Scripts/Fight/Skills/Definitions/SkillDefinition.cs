using DiceBossArena.Game;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill_", menuName = "Dice Boss Arena/Skills/Skill Definition")]
public class SkillDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string skillId;

    [SerializeField]
    private string nameLocalizationKey;

    [SerializeField]
    private string descriptionLocalizationKey;

    [SerializeField]
    private string displayName;

    [SerializeField, TextArea]
    private string description;

    [SerializeField]
    private Sprite icon;

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
    [SerializeField] private List<SkillEffectDefinition> effects = new();

    public LocalizationKey NameLocalizationKey =>
    new LocalizationKey(nameLocalizationKey);

    public LocalizationKey DescriptionLocalizationKey =>
        new LocalizationKey(descriptionLocalizationKey);

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
        skillId =
            skillId?.Trim() ??
            string.Empty;

        nameLocalizationKey =
            nameLocalizationKey?.Trim() ??
            string.Empty;

        descriptionLocalizationKey =
            descriptionLocalizationKey?.Trim() ??
            string.Empty;

        displayName =
            string.IsNullOrWhiteSpace(
                displayName)
                ? skillId
                : displayName.Trim();

        description =
            description?.Trim() ??
            string.Empty;

        minRange =
            Mathf.Max(
                0,
                minRange);
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

#if UNITY_EDITOR
    public void InitializeForTests(
        string newSkillId,
        string newDisplayName = null,
        int newMaxLevel = 1,
        string newNameLocalizationKey = null,
        string newDescriptionLocalizationKey = null,
        string newDescription = null)
    {
        skillId =
            newSkillId?.Trim() ??
            string.Empty;

        nameLocalizationKey =
            newNameLocalizationKey?.Trim() ??
            string.Empty;

        descriptionLocalizationKey =
            newDescriptionLocalizationKey?.Trim() ??
            string.Empty;

        displayName =
            newDisplayName?.Trim() ??
            skillId;

        description =
            newDescription?.Trim() ??
            string.Empty;

        maxLevel =
            Mathf.Max(
                1,
                newMaxLevel);

        levels =
            new List<SkillLevelData>();

        for (int i = 0; i < maxLevel; i++)
        {
            levels.Add(
                SkillLevelData.Default);
        }
    }
#endif
}