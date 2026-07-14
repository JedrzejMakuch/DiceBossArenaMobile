using DiceBossArena.Game;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "StatusEffect_",
    menuName =
        "Dice Boss Arena/Status Effects/Status Effect Definition")]
public sealed class StatusEffectDefinition :
    ScriptableObject
{
    [Header("Identity")]
    [SerializeField]
    private string statusEffectId;

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

    [SerializeField]
    private StatusEffectCategory category =
    StatusEffectCategory.Neutral;

    [Header("Lifetime")]
    [SerializeField, Min(1)]
    private int baseDurationTurns = 1;

    [SerializeField, Min(1)]
    private int maxStacks = 1;

    [SerializeField]
    private StatusEffectStackingPolicy stackingPolicy =
        StatusEffectStackingPolicy.RefreshDuration;

    [SerializeField]
    private StatusEffectTickTiming tickTiming =
        StatusEffectTickTiming.None;

    [Header("Stat Modifiers")]
    [SerializeField]
    private List<StatusEffectStatModifierDefinition>
    statModifiers =
        new();

    [Header("Behaviors")]
    [SerializeField]
    private List<StatusEffectBehaviorDefinition>
    behaviors =
        new();

    public StatusEffectId StatusEffectId =>
        new StatusEffectId(
            statusEffectId);

    public LocalizationKey NameLocalizationKey =>
        new LocalizationKey(
            nameLocalizationKey);

    public LocalizationKey DescriptionLocalizationKey =>
        new LocalizationKey(
            descriptionLocalizationKey);

    public IReadOnlyList<
    StatusEffectBehaviorDefinition> Behaviors =>
        behaviors;

    public StatusEffectCategory Category =>
    category;

    public string DisplayName =>
        displayName;

    public string Description =>
        description;

    public Sprite Icon =>
        icon;

    public int BaseDurationTurns =>
        baseDurationTurns;

    public int MaxStacks =>
        maxStacks;

    public StatusEffectStackingPolicy StackingPolicy =>
        stackingPolicy;

    public StatusEffectTickTiming TickTiming =>
        tickTiming;

    public IReadOnlyList<
    StatusEffectStatModifierDefinition> StatModifiers =>
        statModifiers;


#if UNITY_EDITOR
    public void InitializeForTests(
        string newStatusEffectId,
        string newDisplayName = null,
        int newBaseDurationTurns = 1,
        int newMaxStacks = 1,
        StatusEffectStackingPolicy newStackingPolicy =
            StatusEffectStackingPolicy.RefreshDuration,
        StatusEffectTickTiming newTickTiming =
            StatusEffectTickTiming.None,
        string newNameLocalizationKey = null,
        string newDescriptionLocalizationKey = null,
        string newDescription = null,
        IReadOnlyList<StatusEffectStatModifierDefinition>
        newStatModifiers = null,
        IReadOnlyList<
    StatusEffectBehaviorDefinition>
        newBehaviors = null,
        StatusEffectCategory newCategory =
    StatusEffectCategory.Neutral)
    {
        statusEffectId =
            newStatusEffectId?.Trim() ??
            string.Empty;

        displayName =
            newDisplayName?.Trim() ??
            statusEffectId;

        baseDurationTurns =
            Mathf.Max(
                1,
                newBaseDurationTurns);

        maxStacks =
            Mathf.Max(
                1,
                newMaxStacks);

        stackingPolicy =
            newStackingPolicy;

        tickTiming =
            newTickTiming;

        nameLocalizationKey =
            newNameLocalizationKey?.Trim() ??
            string.Empty;

        descriptionLocalizationKey =
            newDescriptionLocalizationKey?.Trim() ??
            string.Empty;

        description =
            newDescription?.Trim() ??
            string.Empty;

        statModifiers =
    newStatModifiers == null
        ? new List<
            StatusEffectStatModifierDefinition>()
        : new List<
            StatusEffectStatModifierDefinition>(
                newStatModifiers);

        behaviors =
    newBehaviors == null
        ? new List<
            StatusEffectBehaviorDefinition>()
        : new List<
            StatusEffectBehaviorDefinition>(
                newBehaviors);

        category =
    newCategory;
    }
#endif

    private void OnValidate()
    {
        statusEffectId =
            statusEffectId?.Trim() ??
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
                ? statusEffectId
                : displayName.Trim();

        description =
            description?.Trim() ??
            string.Empty;

        baseDurationTurns =
            Mathf.Max(
                1,
                baseDurationTurns);

        maxStacks =
            Mathf.Max(
                1,
                maxStacks);

        statModifiers ??=
    new List<
        StatusEffectStatModifierDefinition>();

        behaviors ??=
    new List<
        StatusEffectBehaviorDefinition>();
    }
}