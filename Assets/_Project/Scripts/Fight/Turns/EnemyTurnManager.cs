using System.Collections;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;

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

        if (turnManager == null ||
            !turnManager.CombatRunning ||
            turnManager.ActiveUnit != enemy ||
            !enemy.IsAlive)
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
}