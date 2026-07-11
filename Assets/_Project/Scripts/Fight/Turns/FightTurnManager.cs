using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class FightTurnManager : MonoBehaviour
{
    [Header("Unit Sources")]
    [SerializeField] private FightDeploymentManager deploymentManager;
    [SerializeField] private EnemySpawnManager enemySpawnManager;

    private readonly List<FightUnit> turnOrder = new();

    private FightUnit activeUnit;
    private int activeUnitIndex = -1;
    private int roundNumber = 0;
    private bool combatRunning;

    public FightUnit ActiveUnit => activeUnit;
    public int RoundNumber => roundNumber;
    public bool CombatRunning => combatRunning;

    public event Action<FightUnit, int> TurnStarted;
    public event Action<FightUnit> TurnEnded;
    public event Action<string> CombatStopped;

    private void Awake()
    {
        combatRunning = false;
        activeUnit = null;
        activeUnitIndex = -1;
        roundNumber = 0;
    }

    private void OnDestroy()
    {
        UnsubscribeFromUnits();
    }

    public void StartCombat()
    {
        if (combatRunning)
        {
            Debug.LogWarning(
                "FightTurnManager: Combat is already running.",
                this);

            return;
        }

        if (!ValidateReferences())
        {
            return;
        }

        BuildTurnOrder();

        if (turnOrder.Count == 0)
        {
            Debug.LogError(
                "FightTurnManager: No units available for combat.",
                this);

            return;
        }

        combatRunning = true;
        roundNumber = 1;
        activeUnitIndex = -1;

        SubscribeToUnits();
        StartNextTurn();

        Debug.Log($"Combat started with {turnOrder.Count} units.");
    }

    public void EndCurrentTurn()
    {
        if (!combatRunning || activeUnit == null)
        {
            return;
        }

        FightUnit endedUnit = activeUnit;

        Debug.Log($"{endedUnit.UnitName} ended turn.");

        TurnEnded?.Invoke(endedUnit);

        StartNextTurn();
    }

    private void BuildTurnOrder()
    {
        turnOrder.Clear();

        FightUnit playerUnit = deploymentManager.PlayerUnit;

        if (playerUnit != null && playerUnit.IsAlive)
        {
            turnOrder.Add(playerUnit);
        }

        Debug.Log(
            $"EnemySpawnManager reports " +
            $"{enemySpawnManager.SpawnedEnemies.Count} spawned enemies.");

        foreach (FightUnit enemy in enemySpawnManager.SpawnedEnemies)
        {
            if (enemy == null)
            {
                Debug.LogWarning(
                    "FightTurnManager: SpawnedEnemies contains a null entry.");

                continue;
            }

            if (!enemy.IsAlive)
            {
                continue;
            }

            turnOrder.Add(enemy);
        }

        turnOrder.Sort((first, second) =>
        {
            int initiativeComparison =
                second.Initiative.CompareTo(first.Initiative);

            if (initiativeComparison != 0)
            {
                return initiativeComparison;
            }

            return first.GetInstanceID().CompareTo(second.GetInstanceID());
        });

        string order = string.Join(
            " -> ",
            turnOrder.Select(unit =>
                $"{unit.UnitName} ({unit.Initiative})"));

        Debug.Log($"Turn order: {order}");
    }

    private void StartNextTurn()
    {
        if (!combatRunning)
        {
            return;
        }

        RemoveInvalidUnits();

        if (turnOrder.Count == 0)
        {
            StopCombat("No units remaining.");
            return;
        }

        int checkedUnits = 0;

        while (checkedUnits < turnOrder.Count)
        {
            activeUnitIndex++;

            if (activeUnitIndex >= turnOrder.Count)
            {
                activeUnitIndex = 0;
                roundNumber++;

                Debug.Log($"Round {roundNumber} started.");
            }

            FightUnit candidate = turnOrder[activeUnitIndex];
            checkedUnits++;

            if (candidate != null && candidate.IsAlive)
            {
                activeUnit = candidate;
                BeginTurn(activeUnit);
                return;
            }
        }

        StopCombat("No living units remaining.");
    }

    private void BeginTurn(FightUnit unit)
    {
        TurnStarted?.Invoke(unit, roundNumber);

        Debug.Log(
            $"Round {roundNumber}: {unit.UnitName}'s turn. " +
            $"Team: {unit.Team}");
    }

    private void HandleUnitDied(FightUnit unit)
    {
        Debug.Log(
            $"FightTurnManager: Removing dead unit {unit.UnitName}.");

        int removedIndex = turnOrder.IndexOf(unit);

        if (removedIndex < 0)
        {
            return;
        }

        turnOrder.RemoveAt(removedIndex);

        if (removedIndex <= activeUnitIndex)
        {
            activeUnitIndex--;
        }

        if (unit == activeUnit)
        {
            activeUnit = null;
            StartNextTurn();
        }
    }

    private void RemoveInvalidUnits()
    {
        for (int i = turnOrder.Count - 1; i >= 0; i--)
        {
            FightUnit unit = turnOrder[i];

            if (unit != null && unit.IsAlive)
            {
                continue;
            }

            turnOrder.RemoveAt(i);

            if (i <= activeUnitIndex)
            {
                activeUnitIndex--;
            }
        }
    }

    private void SubscribeToUnits()
    {
        foreach (FightUnit unit in turnOrder)
        {
            if (unit != null)
            {
                unit.Died += HandleUnitDied;
            }
        }
    }

    private void UnsubscribeFromUnits()
    {
        foreach (FightUnit unit in turnOrder)
        {
            if (unit != null)
            {
                unit.Died -= HandleUnitDied;
            }
        }
    }

    private void StopCombat(string reason)
    {
        combatRunning = false;
        activeUnit = null;

        CombatStopped?.Invoke(reason);

        Debug.Log($"Combat stopped. Reason: {reason}");
    }

    private bool ValidateReferences()
    {
        bool isValid = true;

        if (deploymentManager == null)
        {
            Debug.LogError(
                "FightTurnManager: Deployment Manager is not assigned.",
                this);

            isValid = false;
        }

        if (enemySpawnManager == null)
        {
            Debug.LogError(
                "FightTurnManager: Enemy Spawn Manager is not assigned.",
                this);

            isValid = false;
        }

        return isValid;
    }
}