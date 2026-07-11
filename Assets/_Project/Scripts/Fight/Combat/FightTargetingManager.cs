using System;
using UnityEngine;

public class FightTargetingManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private EnemySpawnManager enemySpawnManager;

    private FightUnit selectedTarget;
    private bool targetingEnabled;

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

        SubscribeToEnemies();
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

        UnsubscribeFromEnemies();
    }

    private void SubscribeToEnemies()
    {
        if (enemySpawnManager == null)
        {
            return;
        }

        foreach (FightUnit enemy in enemySpawnManager.SpawnedEnemies)
        {
            if (enemy == null)
            {
                continue;
            }

            FightUnitClickHandler clickHandler =
                enemy.GetComponent<FightUnitClickHandler>();

            if (clickHandler != null)
            {
                clickHandler.Clicked += HandleUnitClicked;
            }
        }
    }

    private void UnsubscribeFromEnemies()
    {
        if (enemySpawnManager == null)
        {
            return;
        }

        foreach (FightUnit enemy in enemySpawnManager.SpawnedEnemies)
        {
            if (enemy == null)
            {
                continue;
            }

            FightUnitClickHandler clickHandler =
                enemy.GetComponent<FightUnitClickHandler>();

            if (clickHandler != null)
            {
                clickHandler.Clicked -= HandleUnitClicked;
            }
        }
    }

    private void HandleTurnStarted(FightUnit unit, int roundNumber)
    {
        targetingEnabled =
            unit != null &&
            unit.Team == FightTeam.Player;

        ClearTarget();
    }

    private void HandleTurnEnded(FightUnit unit)
    {
        targetingEnabled = false;
        ClearTarget();
    }

    private void HandleCombatStopped(string reason)
    {
        targetingEnabled = false;
        ClearTarget();
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
        if (attacker == null || target == null)
        {
            return false;
        }

        if (!attacker.IsAlive || !target.IsAlive)
        {
            return false;
        }

        if (attacker == target)
        {
            return false;
        }

        if (attacker.Team == target.Team)
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
        UnsubscribeFromEnemies();
        SubscribeToEnemies();
    }
}