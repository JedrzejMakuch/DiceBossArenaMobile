using System;
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

    [Header("Cached Modules")]
    [SerializeField] private FightUnitTurnResources turnResources;
    [SerializeField] private FightUnitSkills skills;

    public string UnitName => definition != null ? definition.UnitName : unitName;
    public FightTeam Team => definition != null ? definition.Team : team;
    public int MaxHealth => definition != null ? definition.MaxHealth : maxHealth;
    public int CurrentHealth => runtimeState != null ? runtimeState.CurrentHealth : 0;
    public int AttackPower => definition != null ? definition.AttackPower : attackPower;
    public int Initiative => definition != null ? definition.Initiative : initiative;

    public FightGridTile CurrentTile => runtimeState != null ? runtimeState.CurrentTile : null;
    public FightUnitTurnResources TurnResources => turnResources;
    public FightUnitSkills Skills => skills;
    public bool IsAlive => runtimeState != null && runtimeState.IsAlive;

    public event Action<FightUnit> HealthChanged;
    public event Action<FightUnit> Died;

    private void Awake()
    {
        CacheModules();
        InitializeRuntimeState();
    }

    public void Initialize(
    string newUnitName,
    FightTeam newTeam,
    int newMaxHealth,
    int newAttackPower,
    int newInitiative)
    {
        CacheModules();

        definition = null;

        unitName = newUnitName;
        team = newTeam;
        maxHealth = Mathf.Max(1, newMaxHealth);
        attackPower = Mathf.Max(0, newAttackPower);
        initiative = Mathf.Max(0, newInitiative);

        InitializeRuntimeState();

        HealthChanged?.Invoke(this);
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

        definition = newDefinition;
        InitializeRuntimeState();

        HealthChanged?.Invoke(this);

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

    private void InitializeRuntimeState()
    {
        if (runtimeState == null)
        {
            runtimeState =
                new FightUnitRuntimeState(MaxHealth);

            return;
        }

        runtimeState.ResetHealth(MaxHealth);
        runtimeState.ClearTile();
    }

    private void CacheModules()
    {
        if (turnResources == null)
        {
            turnResources = GetComponent<FightUnitTurnResources>();
        }

        if (skills == null)
        {
            skills = GetComponent<FightUnitSkills>();
        }
    }
}