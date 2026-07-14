using DiceBossArena.Game;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Unit_",
    menuName = "Dice Boss Arena/Units/Unit Definition")]
public sealed class FightUnitDefinition : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string unitId;

    [SerializeField]
    private string nameLocalizationKey;

    [SerializeField]
    private string descriptionLocalizationKey;

    [SerializeField]
    private string unitName = "Unit";

    [SerializeField, TextArea]
    private string description;

    [SerializeField]
    private FightTeam team;

    [Header("Combat Stats")]
    [SerializeField, Min(1)] private int maxHealth = 10;
    [SerializeField, Min(0)] private int attackPower = 2;
    [SerializeField, Min(0)] private int initiative = 10;

    [Header("Starting Skills")]
    [SerializeField]
    private List<UnitStartingSkill> startingSkills = new();

    public IReadOnlyList<UnitStartingSkill> StartingSkills =>
    startingSkills;

    public FightUnitDefinitionId UnitId =>
    new FightUnitDefinitionId(unitId);

    public LocalizationKey NameLocalizationKey =>
        new LocalizationKey(
            nameLocalizationKey);

    public LocalizationKey DescriptionLocalizationKey =>
        new LocalizationKey(
            descriptionLocalizationKey);

    public string UnitName => unitName;
    public string Description => description;
    public FightTeam Team => team;

    public int MaxHealth => maxHealth;
    public int AttackPower => attackPower;
    public int Initiative => initiative;

    public void InitializeForTests(
    string newUnitName,
    FightTeam newTeam,
    int newMaxHealth,
    int newAttackPower,
    int newInitiative,
    IReadOnlyList<UnitStartingSkill> newStartingSkills = null,
    string newUnitId = null,
    string newNameLocalizationKey = null,
    string newDescriptionLocalizationKey = null,
    string newDescription = null)
    {
        unitId =
            newUnitId?.Trim() ??
            string.Empty;

        nameLocalizationKey =
            newNameLocalizationKey?.Trim() ??
            string.Empty;

        descriptionLocalizationKey =
            newDescriptionLocalizationKey?.Trim() ??
            string.Empty;

        unitName =
            newUnitName?.Trim() ??
            string.Empty;

        description =
            newDescription?.Trim() ??
            string.Empty;
        team = newTeam;
        maxHealth = Mathf.Max(1, newMaxHealth);
        attackPower = Mathf.Max(0, newAttackPower);
        initiative = Mathf.Max(0, newInitiative);

        startingSkills =
            newStartingSkills != null
                ? new List<UnitStartingSkill>(
                    newStartingSkills)
                : new List<UnitStartingSkill>();
    }

    private void OnValidate()
    {
        unitId =
            unitId?.Trim() ??
            string.Empty;

        nameLocalizationKey =
            nameLocalizationKey?.Trim() ??
            string.Empty;

        descriptionLocalizationKey =
            descriptionLocalizationKey?.Trim() ??
            string.Empty;

        unitName =
            string.IsNullOrWhiteSpace(unitName)
                ? "Unit"
                : unitName.Trim();

        description =
            description?.Trim() ??
            string.Empty;

        maxHealth = Mathf.Max(1, maxHealth);
        attackPower = Mathf.Max(0, attackPower);
        initiative = Mathf.Max(0, initiative);

        if (startingSkills == null)
        {
            startingSkills = new List<UnitStartingSkill>();
        }
    }
}