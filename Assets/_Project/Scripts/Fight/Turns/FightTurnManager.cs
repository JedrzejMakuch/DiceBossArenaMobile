using System;
using System.Collections.Generic;
using UnityEngine;

public class FightTurnManager : MonoBehaviour
{
    [Header("Unit Sources")]
    [SerializeField] private FightUnitRegistry unitRegistry;

    private readonly List<FightUnit> turnOrder = new();

    private FightUnit activeUnit;
    private int activeUnitIndex = -1;
    private int roundNumber = 0;
    private bool combatRunning;
    private bool isProcessingStatusEffects;

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
        isProcessingStatusEffects = false;
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
        if (!combatRunning ||
            activeUnit == null)
        {
            return;
        }

        FightUnit endedUnit =
            activeUnit;

        isProcessingStatusEffects = true;

        try
        {
            endedUnit
                .ProcessStatusEffectsAtEndOfTurn();
        }
        finally
        {
            isProcessingStatusEffects = false;
        }

        Debug.Log(
            $"{endedUnit.UnitName} ended turn.");

        TurnEnded?.Invoke(
            endedUnit);

        if (!combatRunning)
        {
            return;
        }

        if (activeUnit == endedUnit)
        {
            activeUnit = null;
        }

        StartNextTurn();
    }

    private void BuildTurnOrder()
    {
        turnOrder.Clear();

        if (unitRegistry == null)
        {
            return;
        }

        turnOrder.AddRange(
            FightTurnOrderBuilder.Build(
                unitRegistry.Units));
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
            EndCombat("No units remaining.");
            return;
        }

        int checkedUnits = 0;

        while (checkedUnits < turnOrder.Count)
        {
            activeUnitIndex++;

            if (activeUnitIndex >= turnOrder.Count)
            {
                roundNumber++;

                RebuildTurnOrderForNextRound();

                if (turnOrder.Count == 0)
                {
                    EndCombat("No units remaining.");
                    return;
                }

                activeUnitIndex = 0;

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

        EndCombat("No living units remaining.");
    }

    private void BeginTurn(
    FightUnit unit)
    {
        if (unit == null ||
            !unit.IsAlive)
        {
            activeUnit = null;
            StartNextTurn();
            return;
        }

        isProcessingStatusEffects = true;

        try
        {
            unit.ProcessStatusEffectsAtStartOfTurn();
        }
        finally
        {
            isProcessingStatusEffects = false;
        }

        if (!unit.IsAlive ||
            activeUnit != unit)
        {
            activeUnit = null;
            StartNextTurn();
            return;
        }

        TurnStarted?.Invoke(
            unit,
            roundNumber);

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

            if (!isProcessingStatusEffects)
            {
                StartNextTurn();
            }
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

    public void EndCombat(string reason)
    {
        if (!combatRunning)
        {
            return;
        }

        combatRunning = false;
        activeUnit = null;

        CombatStopped?.Invoke(reason);

        Debug.Log($"Combat stopped. Reason: {reason}");
    }

    private bool ValidateReferences()
    {
        if (unitRegistry != null)
        {
            return true;
        }

        Debug.LogError(
            "FightTurnManager: FightUnitRegistry is not assigned.",
            this);

        return false;
    }

    private void RebuildTurnOrderForNextRound()
    {
        UnsubscribeFromUnits();

        BuildTurnOrder();
        RemoveInvalidUnits();

        SubscribeToUnits();
    }

#if UNITY_EDITOR
    public void InitializeForTests(
        FightUnitRegistry newUnitRegistry)
    {
        unitRegistry =
            newUnitRegistry;
    }
#endif
}