using System.Collections;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightDeploymentManager deploymentManager;
    [SerializeField] private FightCombatManager combatManager;

    [Header("Turn Settings")]
    [SerializeField, Min(0f)] private float enemyTurnDelay = 0.75f;

    private Coroutine enemyTurnCoroutine;

    private void OnEnable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted += HandleTurnStarted;
        }
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted -= HandleTurnStarted;
        }

        StopEnemyTurnCoroutine();
    }

    private void HandleTurnStarted(FightUnit unit, int roundNumber)
    {
        if (unit == null || unit.Team != FightTeam.Enemy)
        {
            return;
        }

        StopEnemyTurnCoroutine();

        enemyTurnCoroutine = StartCoroutine(
            ExecuteEnemyTurn(unit));
    }

    private IEnumerator ExecuteEnemyTurn(FightUnit enemy)
    {
        Debug.Log(
            $"EnemyTurnManager: {enemy.UnitName} is thinking.");

        yield return new WaitForSeconds(enemyTurnDelay);

        if (!CanContinueEnemyTurn(enemy))
        {
            enemyTurnCoroutine = null;
            yield break;
        }

        FightUnit player = deploymentManager.PlayerUnit;

        if (player != null && player.IsAlive)
        {
            bool attackExecuted =
                combatManager.TryExecuteBasicAttack(
                    enemy,
                    player);

            if (attackExecuted)
            {
                Debug.Log(
                    $"EnemyTurnManager: {enemy.UnitName} attacked " +
                    $"{player.UnitName}.");
            }
        }

        yield return new WaitForSeconds(enemyTurnDelay);

        if (!CanContinueEnemyTurn(enemy))
        {
            enemyTurnCoroutine = null;
            yield break;
        }

        Debug.Log(
            $"EnemyTurnManager: {enemy.UnitName} finished its turn.");

        enemyTurnCoroutine = null;
        turnManager.EndCurrentTurn();
    }

    private void StopEnemyTurnCoroutine()
    {
        if (enemyTurnCoroutine == null)
        {
            return;
        }

        StopCoroutine(enemyTurnCoroutine);
        enemyTurnCoroutine = null;
    }

    private bool CanContinueEnemyTurn(FightUnit enemy)
    {
        if (turnManager == null ||
            deploymentManager == null ||
            combatManager == null)
        {
            return false;
        }

        if (!turnManager.CombatRunning)
        {
            return false;
        }

        if (turnManager.ActiveUnit != enemy)
        {
            return false;
        }

        if (enemy == null || !enemy.IsAlive)
        {
            return false;
        }

        return true;
    }
}