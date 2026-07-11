using System;
using UnityEngine;

public class FightCombatManager : MonoBehaviour
{
    public event Action<FightUnit, FightUnit, int> AttackExecuted;

    public bool TryExecuteBasicAttack(
        FightUnit attacker,
        FightUnit target)
    {
        if (!CanAttack(attacker, target))
        {
            return false;
        }

        int damage = Mathf.Max(0, attacker.AttackPower);

        target.TakeDamage(damage);

        AttackExecuted?.Invoke(
            attacker,
            target,
            damage);

        Debug.Log(
            $"{attacker.UnitName} attacked " +
            $"{target.UnitName} for {damage} damage.");

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

        return true;
    }
}