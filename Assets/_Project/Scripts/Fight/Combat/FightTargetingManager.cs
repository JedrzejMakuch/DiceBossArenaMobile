using System;
using System.Collections.Generic;
using UnityEngine;

public class FightTargetingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private EnemySpawnManager enemySpawnManager;

    private FightUnit selectedTarget;
    private bool targetingEnabled;
    private FightUnit subscribedPlayerUnit;
    private readonly List<FightUnit> subscribedTargetUnits = new();

    public IReadOnlyList<FightUnit> PotentialTargets => subscribedTargetUnits;
    public FightUnit SelectedTarget => selectedTarget;
    public bool TargetingEnabled => targetingEnabled;
    public event Action<FightUnit> TargetSelected;
    public event Action TargetCleared;

    private void OnEnable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted += HandleTurnStarted;
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped += HandleCombatStopped;
        }

        if (enemySpawnManager != null)
        {
            enemySpawnManager.EnemiesSpawned += HandleEnemiesSpawned;
        }

        RefreshPotentialTargetsFromLegacySource();
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted -= HandleTurnStarted;
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -= HandleCombatStopped;
        }

        if (enemySpawnManager != null)
        {
            enemySpawnManager.EnemiesSpawned -= HandleEnemiesSpawned;
        }

        ClearPotentialTargets();
        UnsubscribeFromPlayerUnit();
    }

    public void SetPotentialTargets(
    IEnumerable<FightUnit> units)
    {
        ClearPotentialTargets();

        if (units == null)
        {
            return;
        }

        foreach (FightUnit unit in units)
        {
            if (unit == null ||
                subscribedTargetUnits.Contains(unit))
            {
                continue;
            }

            FightUnitClickHandler clickHandler =
                unit.GetComponent<FightUnitClickHandler>();

            if (clickHandler == null)
            {
                continue;
            }

            clickHandler.Clicked += HandleUnitClicked;
            subscribedTargetUnits.Add(unit);
        }
    }

    private void ClearPotentialTargets()
    {
        foreach (FightUnit unit in subscribedTargetUnits)
        {
            if (unit == null)
            {
                continue;
            }

            FightUnitClickHandler clickHandler =
                unit.GetComponent<FightUnitClickHandler>();

            if (clickHandler != null)
            {
                clickHandler.Clicked -= HandleUnitClicked;
            }
        }

        subscribedTargetUnits.Clear();
    }

    private void HandleTurnStarted(
    FightUnit unit,
    int roundNumber)
    {
        UnsubscribeFromPlayerUnit();

        targetingEnabled =
        unit != null &&
        unit.IsControlledBy(
            FightControllerType.LocalPlayer);

        if (targetingEnabled)
        {
            SubscribeToPlayerUnit(unit);
        }

        ClearTarget();
    }

    private void HandleTurnEnded(
    FightUnit unit)
    {
        targetingEnabled = false;

        UnsubscribeFromPlayerUnit();
        ClearTarget();
    }

    private void HandleCombatStopped(
    string reason)
    {
        targetingEnabled = false;

        UnsubscribeFromPlayerUnit();
        ClearTarget();
    }

    private void SubscribeToPlayerUnit(
    FightUnit unit)
    {
        if (unit == null)
        {
            return;
        }

        FightUnitClickHandler clickHandler =
            unit.GetComponent<FightUnitClickHandler>();

        if (clickHandler == null)
        {
            Debug.LogWarning(
                $"{unit.UnitName} has no " +
                $"{nameof(FightUnitClickHandler)}.",
                unit);

            return;
        }

        clickHandler.Clicked +=
            HandleUnitClicked;

        subscribedPlayerUnit =
            unit;
    }

    private void UnsubscribeFromPlayerUnit()
    {
        if (subscribedPlayerUnit == null)
        {
            return;
        }

        FightUnitClickHandler clickHandler =
            subscribedPlayerUnit.GetComponent<
                FightUnitClickHandler>();

        if (clickHandler != null)
        {
            clickHandler.Clicked -=
                HandleUnitClicked;
        }

        subscribedPlayerUnit = null;
    }

    private void HandleUnitClicked(FightUnit unit)
    {
        if (!targetingEnabled)
        {
            return;
        }

        FightUnit activeUnit = turnManager.ActiveUnit;

        if (!IsValidTarget(activeUnit, unit))
        {
            return;
        }

        selectedTarget = unit;

        Debug.Log(
            $"Target selected: {selectedTarget.UnitName}");

        TargetSelected?.Invoke(selectedTarget);
    }

    public bool IsValidTarget(
    FightUnit attacker,
    FightUnit target)
    {
        if (attacker == null ||
            target == null)
        {
            return false;
        }

        if (!attacker.IsAlive ||
            !target.IsAlive)
        {
            return false;
        }

        if (!attacker.IsHostileTo(target))
        {
            return false;
        }

        return true;
    }

    public void ClearTarget()
    {
        if (selectedTarget == null)
        {
            return;
        }

        selectedTarget = null;
        TargetCleared?.Invoke();
    }

    private void HandleEnemiesSpawned()
    {
        RefreshPotentialTargetsFromLegacySource();
    }

    private void RefreshPotentialTargetsFromLegacySource()
    {
        if (enemySpawnManager == null)
        {
            ClearPotentialTargets();
            return;
        }

        SetPotentialTargets(
            enemySpawnManager.SpawnedEnemies);
    }
}