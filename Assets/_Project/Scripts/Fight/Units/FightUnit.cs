using DiceBossArena.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

public class FightUnit : MonoBehaviour
{
    [Header("Definition")]
    [SerializeField] private FightUnitDefinition definition;
    public FightUnitDefinition Definition => definition;

    [Header("Identity")]
    [SerializeField] private string unitName = "Unit";
    [SerializeField] private FightTeam team;

    [Header("Combat Stats")]
    [SerializeField, Min(1)] private int maxHealth = 10;
    [SerializeField, Min(0)] private int attackPower = 2;
    [SerializeField, Min(0)] private int initiative = 10;

    [Header("Runtime")]
    [SerializeField] private FightUnitRuntimeState runtimeState;
    private FightUnitStats stats;

    private StatusEffectCollection statusEffects;

    private StatusEffectStatModifierBinder
        statusEffectStatModifierBinder;

    private readonly StatusEffectTurnProcessor
    statusEffectTurnProcessor =
        new();

    private readonly StatusEffectBehaviorExecutor
        statusEffectBehaviorExecutor =
            new();

    [Header("Cached Modules")]
    [SerializeField] private FightUnitTurnResources turnResources;
    [SerializeField] private FightUnitSkills skills;

    private CharacterClassId classId;

    private CharacterSpecializationId specializationId;

    private EquipmentLoadoutSnapshot equipmentLoadout =
        new EquipmentLoadoutSnapshot(null);

    private IReadOnlyList<CharacterPassiveId> passiveIds =
        Array.Empty<CharacterPassiveId>();

    public string UnitName => definition != null ? definition.UnitName : unitName;
    public FightTeam Team => definition != null ? definition.Team : team;
    public int MaxHealth =>
    stats != null
        ? Mathf.Max(
            1,
            stats.GetFinalValue(
                FightStatType.MaxHealth))
        : GetBaseMaxHealth();
    public int CurrentHealth => runtimeState != null ? runtimeState.CurrentHealth : 0;
    public int AttackPower => stats != null ? stats.GetFinalValue(FightStatType.AttackPower) : GetBaseAttackPower();
    public int Initiative =>
    stats != null
        ? stats.GetFinalValue(
            FightStatType.Initiative)
        : GetBaseInitiative();
    public FightUnitStats Stats => stats;
    public StatusEffectCollection StatusEffects =>
    statusEffects;

    public FightGridTile CurrentTile => runtimeState != null ? runtimeState.CurrentTile : null;
    public FightUnitTurnResources TurnResources => turnResources;
    public FightUnitSkills Skills => skills;
    public bool IsAlive => runtimeState != null && runtimeState.IsAlive;
    public FightUnitOwnership Ownership => runtimeState != null ? runtimeState.Ownership : null;
    public FightTeamId TeamId => Ownership != null ? Ownership.TeamId : MapLegacyTeam(Team);
    public FightParticipantId ParticipantId => Ownership != null ? Ownership.ParticipantId : default;
    public FightControllerType ControllerType => Ownership != null ? Ownership.ControllerType : FightControllerType.None;

    public CharacterClassId ClassId =>
    classId;

    public CharacterSpecializationId SpecializationId =>
        specializationId;

    public EquipmentLoadoutSnapshot EquipmentLoadout =>
        equipmentLoadout;

    public IReadOnlyList<CharacterPassiveId> PassiveIds =>
        passiveIds;

    public bool IsControlledBy(
    FightControllerType controller)
    {
        return ControllerType == controller;
    }

    public bool IsAlliedWith(
        FightUnit other)
    {
        return other != null &&
               TeamId == other.TeamId;
    }

    public bool IsHostileTo(
        FightUnit other)
    {
        return other != null &&
               TeamId != other.TeamId;
    }

    public event Action<FightUnit> HealthChanged;
    public event Action<FightUnit> Died;

    private void Awake()
    {
        CacheModules();
        EnsureStatusEffects();
        InitializeStats();
        InitializeRuntimeState();
        InitializeSkillsFromDefinition();
    }

    public void Initialize(
    string newUnitName,
    FightTeam newTeam,
    int newMaxHealth,
    int newAttackPower,
    int newInitiative)
    {
        CacheModules();
        ResetStatusEffects();

        definition = null;

        unitName = newUnitName;
        team = newTeam;
        maxHealth = Mathf.Max(1, newMaxHealth);
        attackPower = Mathf.Max(0, newAttackPower);
        initiative = Mathf.Max(0, newInitiative);

        InitializeStats();
        InitializeRuntimeState();

        HealthChanged?.Invoke(this);
    }

    private void InitializeSkillsFromDefinition()
    {
        if (definition == null ||
            skills == null)
        {
            return;
        }

        skills.InitializeFromDefinition(
            definition.StartingSkills);
    }

    public bool Initialize(
    FightUnitDefinition newDefinition)
    {
        if (newDefinition == null)
        {
            Debug.LogError(
                "FightUnit cannot initialize without a definition.",
                this);

            return false;
        }

        CacheModules();
        ResetStatusEffects();

        definition = newDefinition;
        InitializeStats();
        InitializeRuntimeState();
        InitializeSkillsFromDefinition();

        HealthChanged?.Invoke(this);

        return true;
    }

    public bool Initialize(
    FightUnitDefinition newDefinition,
    FightUnitOwnership newOwnership)
    {
        if (!Initialize(newDefinition))
        {
            return false;
        }

        if (newOwnership != null)
        {
            runtimeState.AssignOwnership(
                newOwnership);
        }

        return true;
    }

    public bool TryAssignToTile(FightGridTile tile)
    {
        if (tile == null)
        {
            return false;
        }

        if (runtimeState == null)
        {
            InitializeRuntimeState();
        }

        if (runtimeState.CurrentTile == tile)
        {
            return true;
        }

        if (!tile.TryOccupy(this))
        {
            Debug.LogWarning(
                $"{UnitName} cannot occupy tile " +
                $"({tile.GridX}, {tile.GridY}).");

            return false;
        }

        FightGridTile previousTile =
            runtimeState.CurrentTile;

        runtimeState.AssignTile(tile);

        if (previousTile != null)
        {
            previousTile.TryRelease(this);
        }

        transform.position =
            runtimeState.CurrentTile.GetStandPosition();

        Debug.Log(
            $"{UnitName} assigned to tile " +
            $"({runtimeState.CurrentTile.GridX}, " +
            $"{runtimeState.CurrentTile.GridY}).");

        return true;
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive)
        {
            return;
        }

        int damage =
            runtimeState.ApplyDamage(amount);

        HealthChanged?.Invoke(this);

        Debug.Log(
            $"{UnitName} took {damage} damage. " +
            $"HP: {CurrentHealth}/{MaxHealth}");

        if (!IsAlive)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (!IsAlive)
        {
            return;
        }

        runtimeState.ApplyHealing(
            amount,
            MaxHealth);

        HealthChanged?.Invoke(this);
    }

    public StatusEffectApplyResult ApplyStatusEffect(
    StatusEffectDefinition statusEffectDefinition)
    {
        EnsureStatusEffects();

        return statusEffects.Apply(
            statusEffectDefinition);
    }

    public bool RemoveStatusEffect(
        StatusEffectId statusEffectId)
    {
        EnsureStatusEffects();

        return statusEffects.Remove(
            statusEffectId);
    }

    public StatusEffectDispelResult DispelStatusEffects(
    StatusEffectCategory category,
    int maximumEffects = int.MaxValue)
    {
        EnsureStatusEffects();

        return statusEffects.Dispel(
            category,
            maximumEffects);
    }

    public bool HasStatusEffect(
        StatusEffectId statusEffectId)
    {
        EnsureStatusEffects();

        return statusEffects.Contains(
            statusEffectId);
    }

    public StatusEffectTurnProcessResult
    ProcessStatusEffectsAtStartOfTurn()
    {
        EnsureStatusEffects();

        return statusEffectTurnProcessor
            .ProcessStartOfTurn(
                statusEffects,
                ExecuteStatusEffectBehaviors);
    }

    public StatusEffectTurnProcessResult
        ProcessStatusEffectsAtEndOfTurn()
    {
        EnsureStatusEffects();

        return statusEffectTurnProcessor
            .ProcessEndOfTurn(
                statusEffects,
                ExecuteStatusEffectBehaviors);
    }

    private void ExecuteStatusEffectBehaviors(
    StatusEffectRuntimeState state)
    {
        if (!IsAlive ||
            state == null ||
            state.IsExpired)
        {
            return;
        }

        statusEffectBehaviorExecutor.Execute(
            this,
            state);
    }

    private void Die()
    {
        if (runtimeState.CurrentTile != null)
        {
            runtimeState.CurrentTile.TryRelease(this);
            runtimeState.ClearTile();
        }

        Died?.Invoke(this);

        Debug.Log($"{UnitName} died.");

        gameObject.SetActive(false);
    }

    public void ReleaseCurrentTile()
    {
        if (runtimeState == null ||
            runtimeState.CurrentTile == null)
        {
            return;
        }

        runtimeState.CurrentTile.TryRelease(this);
        runtimeState.ClearTile();
    }

    private static FightTeamId MapLegacyTeam(
    FightTeam legacyTeam)
    {
        return legacyTeam == FightTeam.Player
            ? FightTeamId.TeamA
            : FightTeamId.TeamB;
    }

    private FightUnitOwnership CreateLegacyOwnership()
    {
        if (Team == FightTeam.Player)
        {
            return new FightUnitOwnership(
                FightTeamId.TeamA,
                new FightParticipantId("local-player"),
                FightControllerType.LocalPlayer);
        }

        return new FightUnitOwnership(
            FightTeamId.TeamB,
            new FightParticipantId("enemy-ai"),
            FightControllerType.AI);
    }

    private void InitializeStats()
    {
        statusEffectStatModifierBinder?.Dispose();

        statusEffectStatModifierBinder = null;

        if (stats != null)
        {
            stats.StatChanged -= HandleStatChanged;
        }

        Dictionary<FightStatType, int> baseValues =
            new()
            {
            {
                FightStatType.MaxHealth,
                GetBaseMaxHealth()
            },
            {
                FightStatType.AttackPower,
                GetBaseAttackPower()
            },
            {
                FightStatType.MaxActionPoints,
                turnResources != null
                    ? turnResources.ConfiguredMaxActionPoints
                    : 0
            },
            {
                FightStatType.MaxMovementPoints,
                turnResources != null
                    ? turnResources.ConfiguredMaxMovementPoints
                    : 0
            },
            {
                FightStatType.Initiative,
                GetBaseInitiative()
            }
            };

        stats =
            new FightUnitStats(baseValues);

        stats.StatChanged += HandleStatChanged;

        if (turnResources != null)
        {
            turnResources.RefreshStatsSubscription();
        }

        EnsureStatusEffects();

        statusEffectStatModifierBinder =
            new StatusEffectStatModifierBinder(
                statusEffects,
                stats);
    }

    private void HandleStatChanged(
    FightStatType statType)
    {
        if (statType != FightStatType.MaxHealth ||
            runtimeState == null)
        {
            return;
        }

        bool healthChanged =
            runtimeState.ClampHealthToMaximum(
                MaxHealth);

        if (healthChanged)
        {
            HealthChanged?.Invoke(this);
        }
    }

    private void InitializeRuntimeState()
    {
        if (runtimeState == null)
        {
            runtimeState =
                new FightUnitRuntimeState(MaxHealth);
        }
        else
        {
            runtimeState.ResetHealth(MaxHealth);
            runtimeState.ClearTile();
        }

        runtimeState.AssignOwnership(
            CreateLegacyOwnership());
    }

    private void CacheModules()
    {
        if (turnResources == null)
        {
            turnResources =
                GetComponent<FightUnitTurnResources>();
        }

        if (turnResources != null)
        {
            turnResources.Initialize(this);
        }

        if (skills == null)
        {
            skills =
                GetComponent<FightUnitSkills>();
        }
    }

    private int GetBaseAttackPower()
    {
        return definition != null
            ? definition.AttackPower
            : attackPower;
    }

    private int GetBaseInitiative()
    {
        return definition != null
            ? definition.Initiative
            : initiative;
    }

    private int GetBaseMaxHealth()
    {
        return definition != null
            ? definition.MaxHealth
            : maxHealth;
    }

    private void EnsureStatusEffects()
    {
        statusEffects ??=
            new StatusEffectCollection();
    }

    private void ResetStatusEffects()
    {
        EnsureStatusEffects();

        statusEffects.Clear();
    }

    private void OnDestroy()
    {
        statusEffectStatModifierBinder?.Dispose();

        statusEffectStatModifierBinder = null;

        if (stats != null)
        {
            stats.StatChanged -= HandleStatChanged;
        }
    }

    public bool ApplyBuild(
    ResolvedCharacterBuild build)
    {
        if (build == null)
        {
            return false;
        }

        CacheModules();

        classId =
            build.ClassId;

        specializationId =
            build.SpecializationId;

        equipmentLoadout =
            build.EquipmentLoadout ??
            new EquipmentLoadoutSnapshot(null);

        passiveIds =
            CopyPassiveIds(
                build.PassiveIds);

        if (skills != null)
        {
            skills.InitializeFromDefinition(
                build.Skills);
        }

        if (stats != null)
        {
            ApplyStatModifiers(
                build.StatModifiers);
        }

        return true;
    }

    private static IReadOnlyList<CharacterPassiveId>
    CopyPassiveIds(
        IReadOnlyList<CharacterPassiveId> source)
    {
        if (source == null ||
            source.Count == 0)
        {
            return Array.Empty<CharacterPassiveId>();
        }

        CharacterPassiveId[] result =
            new CharacterPassiveId[source.Count];

        for (int i = 0; i < source.Count; i++)
        {
            result[i] = source[i];
        }

        return Array.AsReadOnly(result);
    }

    private void ApplyStatModifiers(
        IReadOnlyList<FightStatModifier> modifiers)
    {
        if (stats == null ||
            modifiers == null)
        {
            return;
        }

        for (int i = 0; i < modifiers.Count; i++)
        {
            stats.AddModifier(
                modifiers[i]);
        }
    }

    public bool ApplyRuntimeSnapshot(
    FightUnitRuntimeSnapshot snapshot)
    {
        if (snapshot == null)
        {
            return false;
        }

        if (runtimeState == null)
        {
            InitializeRuntimeState();
        }

        if (!snapshot.HasCurrentHealth)
        {
            runtimeState.ResetHealth(
                MaxHealth);

            HealthChanged?.Invoke(this);

            return true;
        }

        runtimeState.RestoreCurrentHealth(
            snapshot.CurrentHealth,
            MaxHealth);

        HealthChanged?.Invoke(this);

        return true;
    }
}