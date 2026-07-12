using System;
using UnityEngine;

public class FightCombatManager : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField, Min(1)] private int basicAttackActionPointCost = 1;

    public int BasicAttackActionPointCost => basicAttackActionPointCost;

    public event Action<FightUnit, FightUnit, int> AttackExecuted;

    public bool TryExecuteBasicAttack(
        FightUnit attacker,
        FightUnit target)
    {
        if (!CanAttack(attacker, target))
        {
            return false;
        }

        FightUnitTurnResources resources =
            attacker.GetComponent<FightUnitTurnResources>();

        if (resources == null)
        {
            Debug.LogError(
                $"FightCombatManager: {attacker.UnitName} " +
                $"has no FightUnitTurnResources.",
                attacker);

            return false;
        }

        if (!resources.TrySpendActionPoints(
                basicAttackActionPointCost))
        {
            Debug.LogWarning(
                $"{attacker.UnitName} cannot execute basic attack. " +
                $"Required AP: {basicAttackActionPointCost}, " +
                $"available AP: {resources.CurrentActionPoints}.",
                attacker);

            return false;
        }

        int damage = Mathf.Max(
            0,
            attacker.AttackPower);

        target.TakeDamage(damage);

        AttackExecuted?.Invoke(
            attacker,
            target,
            damage);

        Debug.Log(
            $"{attacker.UnitName} attacked " +
            $"{target.UnitName} for {damage} damage. " +
            $"Cost: {basicAttackActionPointCost} AP. " +
            $"Remaining AP: {resources.CurrentActionPoints}.");

        return true;
    }

    private bool CanAttack(
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

        FightUnitTurnResources resources =
            attacker.GetComponent<FightUnitTurnResources>();

        if (resources == null)
        {
            return false;
        }

        return resources.CanSpendActionPoints(
            basicAttackActionPointCost);
    }
}