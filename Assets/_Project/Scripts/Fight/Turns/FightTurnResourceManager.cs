using System;
using UnityEngine;

public class FightTurnResourceManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;

    public event Action<FightUnit> TurnResourcesReady;

    private void OnEnable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted += HandleTurnStarted;
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped += HandleCombatStopped;
        }
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted -= HandleTurnStarted;
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -= HandleCombatStopped;
        }
    }

    private void HandleTurnStarted(
    FightUnit unit,
    int roundNumber)
    {
        if (unit == null || !unit.IsAlive)
        {
            return;
        }

        FightUnitTurnResources resources =
            unit.TurnResources;

        if (resources == null)
        {
            Debug.LogError(
                $"FightTurnResourceManager: " +
                $"{unit.UnitName} has no FightUnitTurnResources.",
                unit);

            return;
        }

        resources.ResetForTurn();

        TurnResourcesReady?.Invoke(unit);
    }

    private void HandleTurnEnded(FightUnit unit)
    {
        ClearUnitResources(unit);
    }

    private void HandleCombatStopped(string reason)
    {
        FightUnit activeUnit = turnManager != null
            ? turnManager.ActiveUnit
            : null;

        ClearUnitResources(activeUnit);
    }

    private void ClearUnitResources(FightUnit unit)
    {
        if (unit == null)
        {
            return;
        }

        FightUnitTurnResources resources = unit.TurnResources;

        if (resources != null)
        {
            resources.ClearResources();
        }
    }
}