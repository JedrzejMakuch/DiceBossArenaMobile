using DiceBossArena.Game;
using System;
using UnityEngine;

public class FightUnitTurnResources : MonoBehaviour
{
    [Header("Action Points")]
    [SerializeField, Min(0)] private int maxActionPoints = 1;

    [Header("Movement Points")]
    [SerializeField, Min(0)] private int maxMovementPoints = 4;

    [SerializeField] private FightUnit owner;

    public int MaxActionPoints =>
    owner != null &&
    owner.Stats != null
        ? Mathf.Max(
            0,
            owner.Stats.GetFinalValue(
                FightStatType.MaxActionPoints))
        : maxActionPoints;

    public int CurrentActionPoints { get; private set; }

    public int MaxMovementPoints =>
    owner != null &&
    owner.Stats != null
        ? Mathf.Max(
            0,
            owner.Stats.GetFinalValue(
                FightStatType.MaxMovementPoints))
        : maxMovementPoints;

    public int CurrentMovementPoints { get; private set; }

    public bool HasActionPoints =>
        CurrentActionPoints > 0;

    public bool HasMovementPoints =>
        CurrentMovementPoints > 0;

    public int ConfiguredMaxActionPoints =>
    maxActionPoints;

    public int ConfiguredMaxMovementPoints =>
        maxMovementPoints;

    public event Action<FightUnitTurnResources> ResourcesReset;
    public event Action<FightUnitTurnResources> ResourcesChanged;

    private void Awake()
    {
        if (owner == null)
        {
            owner =
                GetComponent<FightUnit>();
        }

        ClearResources();
    }

    public void ResetForTurn()
    {
        CurrentActionPoints = MaxActionPoints;
        CurrentMovementPoints = MaxMovementPoints;

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

    private void OnEnable()
    {
        SubscribeToStats();
    }

    private void OnDisable()
    {
        UnsubscribeFromStats();
    }

    private void SubscribeToStats()
    {
        if (owner == null)
        {
            owner =
                GetComponent<FightUnit>();
        }

        if (owner != null &&
            owner.Stats != null)
        {
            owner.Stats.StatChanged +=
                HandleStatChanged;
        }
    }

    private void UnsubscribeFromStats()
    {
        if (owner != null &&
            owner.Stats != null)
        {
            owner.Stats.StatChanged -=
                HandleStatChanged;
        }
    }

    private void HandleStatChanged(
        FightStatType statType)
    {
        bool changed = false;

        if (statType == FightStatType.MaxActionPoints &&
            CurrentActionPoints > MaxActionPoints)
        {
            CurrentActionPoints =
                MaxActionPoints;

            changed = true;
        }

        if (statType == FightStatType.MaxMovementPoints &&
            CurrentMovementPoints > MaxMovementPoints)
        {
            CurrentMovementPoints =
                MaxMovementPoints;

            changed = true;
        }

        if (changed)
        {
            ResourcesChanged?.Invoke(this);
        }
    }

    public void RefreshStatsSubscription()
    {
        UnsubscribeFromStats();
        SubscribeToStats();
    }
}