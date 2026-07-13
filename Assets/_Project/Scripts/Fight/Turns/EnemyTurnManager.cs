using System.Collections;
using UnityEngine;

public class EnemyTurnManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightDeploymentManager deploymentManager;
    [SerializeField] private FightSkillManager skillManager;

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

    private void HandleTurnStarted(
        FightUnit unit,
        int roundNumber)
    {
        if (unit == null ||
            unit.Team != FightTeam.Enemy)
        {
            return;
        }

        StopEnemyTurnCoroutine();

        enemyTurnCoroutine =
            StartCoroutine(ExecuteEnemyTurn(unit));
    }

    private IEnumerator ExecuteEnemyTurn(
        FightUnit enemy)
    {
        Debug.Log(
            $"EnemyTurnManager: {enemy.UnitName} is thinking.");

        yield return new WaitForSeconds(enemyTurnDelay);

        if (!CanContinueEnemyTurn(enemy, true))
        {
            enemyTurnCoroutine = null;
            yield break;
        }

        FightUnit player =
            deploymentManager.PlayerUnit;

        FightUnitSkills enemySkills = enemy.Skills;

        UnitSkillState basicAttack =
            enemySkills != null
                ? enemySkills.GetSkillById("basic_attack")
                : null;

        if (player != null &&
            player.IsAlive &&
            basicAttack != null)
        {
            bool skillExecuted =
                skillManager.TryExecuteSkill(
                    enemy,
                    basicAttack,
                    player);

            if (skillExecuted)
            {
                Debug.Log(
                    $"EnemyTurnManager: {enemy.UnitName} used " +
                    $"{basicAttack.Definition.DisplayName} on " +
                    $"{player.UnitName}.");
            }
            else
            {
                Debug.LogWarning(
                    $"EnemyTurnManager: {enemy.UnitName} " +
                    $"failed to execute Basic Attack.",
                    enemy);
            }
        }
        else
        {
            Debug.LogWarning(
                $"EnemyTurnManager: {enemy.UnitName} " +
                $"could not find a valid Basic Attack or player target.",
                enemy);
        }

        yield return new WaitForSeconds(enemyTurnDelay);

        if (!CanContinueEnemyTurn(enemy, true))
        {
            enemyTurnCoroutine = null;
            yield break;
        }

        Debug.Log(
            $"EnemyTurnManager: {enemy.UnitName} finished its turn.");

        enemyTurnCoroutine = null;

        if (turnManager.CombatRunning &&
            turnManager.ActiveUnit == enemy)
        {
            turnManager.EndCurrentTurn();
        }
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

    private bool CanContinueEnemyTurn(
        FightUnit enemy,
        bool logReason = false)
    {
        if (enemy == null)
        {
            if (logReason)
            {
                Debug.LogWarning(
                    "EnemyTurnManager: enemy reference is null.",
                    this);
            }

            return false;
        }

        if (turnManager == null)
        {
            if (logReason)
            {
                Debug.LogWarning(
                    "EnemyTurnManager: Turn Manager is not assigned.",
                    this);
            }

            return false;
        }

        if (deploymentManager == null)
        {
            if (logReason)
            {
                Debug.LogWarning(
                    "EnemyTurnManager: Deployment Manager is not assigned.",
                    this);
            }

            return false;
        }

        if (skillManager == null)
        {
            if (logReason)
            {
                Debug.LogWarning(
                    "EnemyTurnManager: Skill Manager is not assigned.",
                    this);
            }

            return false;
        }

        if (!turnManager.CombatRunning)
        {
            if (logReason)
            {
                Debug.LogWarning(
                    $"EnemyTurnManager: combat stopped during " +
                    $"{enemy.UnitName}'s turn.",
                    enemy);
            }

            return false;
        }

        if (!enemy.IsAlive)
        {
            if (logReason)
            {
                Debug.LogWarning(
                    $"EnemyTurnManager: {enemy.UnitName} is no longer alive.",
                    enemy);
            }

            return false;
        }

        if (turnManager.ActiveUnit != enemy)
        {
            if (logReason)
            {
                string activeUnitName =
                    turnManager.ActiveUnit != null
                        ? turnManager.ActiveUnit.UnitName
                        : "null";

                Debug.LogWarning(
                    $"EnemyTurnManager: active unit changed. " +
                    $"Expected: {enemy.UnitName}, " +
                    $"actual: {activeUnitName}.",
                    enemy);
            }

            return false;
        }

        return true;
    }
}