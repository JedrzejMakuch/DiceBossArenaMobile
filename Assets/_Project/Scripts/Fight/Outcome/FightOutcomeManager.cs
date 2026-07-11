using System;
using System.Collections.Generic;
using UnityEngine;

public class FightOutcomeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightDeploymentManager deploymentManager;
    [SerializeField] private EnemySpawnManager enemySpawnManager;

    private readonly HashSet<FightUnit> subscribedUnits = new();

    public FightOutcome CurrentOutcome { get; private set; }
        = FightOutcome.None;

    public bool FightFinished =>
        CurrentOutcome != FightOutcome.None;

    public event Action<FightOutcome> OutcomeResolved;

    private void OnEnable()
    {
        if (enemySpawnManager != null)
        {
            enemySpawnManager.EnemiesSpawned += HandleEnemiesSpawned;
        }
    }

    private void OnDisable()
    {
        if (enemySpawnManager != null)
        {
            enemySpawnManager.EnemiesSpawned -= HandleEnemiesSpawned;
        }

        UnsubscribeFromUnits();
    }

    private void HandleEnemiesSpawned()
    {
        UnsubscribeFromUnits();
        SubscribeToUnits();

        CurrentOutcome = FightOutcome.None;

        Debug.Log(
            $"FightOutcomeManager: Monitoring " +
            $"{subscribedUnits.Count} units.");
    }

    private void SubscribeToUnits()
    {
        FightUnit player = deploymentManager != null
            ? deploymentManager.PlayerUnit
            : null;

        SubscribeToUnit(player);

        if (enemySpawnManager == null)
        {
            return;
        }

        foreach (FightUnit enemy in enemySpawnManager.SpawnedEnemies)
        {
            SubscribeToUnit(enemy);
        }
    }

    private void SubscribeToUnit(FightUnit unit)
    {
        if (unit == null || subscribedUnits.Contains(unit))
        {
            return;
        }

        unit.Died += HandleUnitDied;
        subscribedUnits.Add(unit);
    }

    private void UnsubscribeFromUnits()
    {
        foreach (FightUnit unit in subscribedUnits)
        {
            if (unit != null)
            {
                unit.Died -= HandleUnitDied;
            }
        }

        subscribedUnits.Clear();
    }

    private void HandleUnitDied(FightUnit deadUnit)
    {
        if (FightFinished)
        {
            return;
        }

        Debug.Log(
            $"FightOutcomeManager: Checking outcome after " +
            $"{deadUnit.UnitName} died.");

        EvaluateOutcome();
    }

    private void EvaluateOutcome()
    {
        FightUnit player = deploymentManager != null
            ? deploymentManager.PlayerUnit
            : null;

        if (player == null || !player.IsAlive)
        {
            ResolveOutcome(FightOutcome.Defeat);
            return;
        }

        bool anyEnemyAlive = false;

        if (enemySpawnManager != null)
        {
            foreach (FightUnit enemy in enemySpawnManager.SpawnedEnemies)
            {
                if (enemy != null && enemy.IsAlive)
                {
                    anyEnemyAlive = true;
                    break;
                }
            }
        }

        if (!anyEnemyAlive)
        {
            ResolveOutcome(FightOutcome.Victory);
        }
    }

    public void ForceVictory()
    {
        ResolveOutcome(FightOutcome.Victory);
    }

    public void ForceDefeat()
    {
        ResolveOutcome(FightOutcome.Defeat);
    }

    private void ResolveOutcome(FightOutcome outcome)
    {
        if (FightFinished || outcome == FightOutcome.None)
        {
            return;
        }

        CurrentOutcome = outcome;

        if (turnManager != null)
        {
            turnManager.EndCombat(outcome.ToString());
        }
        else
        {
            Debug.LogError(
                "FightOutcomeManager: Turn Manager is not assigned.",
                this);
        }

        OutcomeResolved?.Invoke(outcome);

        Debug.Log($"Fight resolved: {outcome}");
    }
}