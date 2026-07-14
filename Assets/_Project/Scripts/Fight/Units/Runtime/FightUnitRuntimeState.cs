using System;
using UnityEngine;

[Serializable]
public sealed class FightUnitRuntimeState
{
    [SerializeField] private int currentHealth;
    [SerializeField] private FightGridTile currentTile;
    [SerializeField]
    private FightUnitOwnership ownership;

    public int CurrentHealth => currentHealth;
    public FightGridTile CurrentTile => currentTile;
    public bool IsAlive => currentHealth > 0;
    public FightUnitOwnership Ownership => ownership;

    public FightUnitRuntimeState(int maxHealth)
    {
        currentHealth = Mathf.Max(1, maxHealth);
    }

    public void ResetHealth(int maxHealth)
    {
        currentHealth = Mathf.Max(1, maxHealth);
    }

    public void RestoreCurrentHealth(
        int restoredHealth,
        int maxHealth)
    {
        int clampedMaximum =
            Mathf.Max(1, maxHealth);

        currentHealth =
            Mathf.Clamp(
                restoredHealth,
                0,
                clampedMaximum);
    }

    public int ApplyDamage(int amount)
    {
        int damage = Mathf.Max(0, amount);
        currentHealth = Mathf.Max(0, currentHealth - damage);

        return damage;
    }

    public int ApplyHealing(
        int amount,
        int maxHealth)
    {
        int healing = Mathf.Max(0, amount);
        currentHealth = Mathf.Min(
            Mathf.Max(1, maxHealth),
            currentHealth + healing);

        return healing;
    }

    public void AssignTile(FightGridTile tile)
    {
        currentTile = tile;
    }

    public void ClearTile()
    {
        currentTile = null;
    }

    public void AssignOwnership(
    FightUnitOwnership newOwnership)
    {
        ownership = newOwnership;
    }

    public bool ClampHealthToMaximum(
    int maxHealth)
    {
        int clampedMaximum =
            Mathf.Max(1, maxHealth);

        int previousHealth =
            currentHealth;

        currentHealth =
            Mathf.Clamp(
                currentHealth,
                0,
                clampedMaximum);

        return currentHealth != previousHealth;
    }
}