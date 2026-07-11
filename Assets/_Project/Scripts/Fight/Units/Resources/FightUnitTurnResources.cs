using System;
using UnityEngine;

public class FightUnitTurnResources : MonoBehaviour
{
    [Header("Action Points")]
    [SerializeField, Min(0)] private int maxActionPoints = 1;

    [Header("Movement Points")]
    [SerializeField, Min(0)] private int maxMovementPoints = 4;

    public int MaxActionPoints => maxActionPoints;
    public int CurrentActionPoints { get; private set; }

    public int MaxMovementPoints => maxMovementPoints;
    public int CurrentMovementPoints { get; private set; }

    public bool HasActionPoints =>
        CurrentActionPoints > 0;

    public bool HasMovementPoints =>
        CurrentMovementPoints > 0;

    public event Action<FightUnitTurnResources> ResourcesReset;
    public event Action<FightUnitTurnResources> ResourcesChanged;

    private void Awake()
    {
        ClearResources();
    }

    public void ResetForTurn()
    {
        CurrentActionPoints = maxActionPoints;
        CurrentMovementPoints = maxMovementPoints;

        ResourcesReset?.Invoke(this);
        ResourcesChanged?.Invoke(this);

        Debug.Log(
            $"{name}: turn resources reset. " +
            $"AP: {CurrentActionPoints}/{MaxActionPoints}, " +
            $"MP: {CurrentMovementPoints}/{MaxMovementPoints}");
    }

    public bool TrySpendActionPoints(int amount)
    {
        if (!CanSpendActionPoints(amount))
        {
            return false;
        }

        CurrentActionPoints -= amount;
        ResourcesChanged?.Invoke(this);

        Debug.Log(
            $"{name}: spent {amount} AP. " +
            $"Remaining: {CurrentActionPoints}/{MaxActionPoints}");

        return true;
    }

    public bool TrySpendMovementPoints(int amount)
    {
        if (!CanSpendMovementPoints(amount))
        {
            return false;
        }

        CurrentMovementPoints -= amount;
        ResourcesChanged?.Invoke(this);

        Debug.Log(
            $"{name}: spent {amount} MP. " +
            $"Remaining: {CurrentMovementPoints}/{MaxMovementPoints}");

        return true;
    }

    public bool CanSpendActionPoints(int amount)
    {
        return amount > 0 &&
               CurrentActionPoints >= amount;
    }

    public bool CanSpendMovementPoints(int amount)
    {
        return amount > 0 &&
               CurrentMovementPoints >= amount;
    }

    public void ClearResources()
    {
        CurrentActionPoints = 0;
        CurrentMovementPoints = 0;

        ResourcesChanged?.Invoke(this);
    }
}