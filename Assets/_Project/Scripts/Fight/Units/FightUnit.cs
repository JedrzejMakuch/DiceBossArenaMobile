using System;
using UnityEngine;

public class FightUnit : MonoBehaviour
{
    [Header("Identity")]
    [SerializeField] private string unitName = "Unit";
    [SerializeField] private FightTeam team;

    [Header("Combat Stats")]
    [SerializeField, Min(1)] private int maxHealth = 10;
    [SerializeField, Min(0)] private int attackPower = 2;
    [SerializeField, Min(0)] private int initiative = 10;

    [Header("Runtime")]
    [SerializeField] private int currentHealth;
    [SerializeField] private FightGridTile currentTile;

    [Header("Cached Modules")]
    [SerializeField] private FightUnitTurnResources turnResources;
    [SerializeField] private FightUnitSkills skills;

    public string UnitName => unitName;
    public FightTeam Team => team;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    public int AttackPower => attackPower;
    public int Initiative => initiative;

    public FightGridTile CurrentTile => currentTile;
    public FightUnitTurnResources TurnResources => turnResources;
    public FightUnitSkills Skills => skills;
    public bool IsAlive => currentHealth > 0;

    public event Action<FightUnit> HealthChanged;
    public event Action<FightUnit> Died;

    private void Awake()
    {
        CacheModules();
        currentHealth = maxHealth;
    }

    public void Initialize(
    string newUnitName,
    FightTeam newTeam,
    int newMaxHealth,
    int newAttackPower,
    int newInitiative)
    {
        CacheModules();

        unitName = newUnitName;
        team = newTeam;
        maxHealth = Mathf.Max(1, newMaxHealth);
        attackPower = Mathf.Max(0, newAttackPower);
        initiative = Mathf.Max(0, newInitiative);
        currentHealth = maxHealth;

        HealthChanged?.Invoke(this);
    }

    public bool TryAssignToTile(FightGridTile tile)
    {
        if (tile == null)
        {
            return false;
        }

        if (currentTile == tile)
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

        FightGridTile previousTile = currentTile;
        currentTile = tile;

        if (previousTile != null)
        {
            previousTile.TryRelease(this);
        }

        transform.position = currentTile.GetStandPosition();

        Debug.Log(
            $"{UnitName} assigned to tile " +
            $"({currentTile.GridX}, {currentTile.GridY}).");

        return true;
    }

    public void TakeDamage(int amount)
    {
        if (!IsAlive)
        {
            return;
        }

        int damage = Mathf.Max(0, amount);
        currentHealth = Mathf.Max(0, currentHealth - damage);

        HealthChanged?.Invoke(this);

        Debug.Log(
            $"{unitName} took {damage} damage. " +
            $"HP: {currentHealth}/{maxHealth}");

        if (currentHealth == 0)
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

        int healAmount = Mathf.Max(0, amount);
        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);

        HealthChanged?.Invoke(this);
    }

    private void Die()
    {
        if (currentTile != null)
        {
            currentTile.TryRelease(this);
            currentTile = null;
        }

        Died?.Invoke(this);

        Debug.Log($"{unitName} died.");

        gameObject.SetActive(false);
    }

    public void ReleaseCurrentTile()
    {
        if (currentTile == null)
        {
            return;
        }

        currentTile.TryRelease(this);
        currentTile = null;
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