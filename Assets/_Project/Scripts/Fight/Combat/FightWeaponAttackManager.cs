using DiceBossArena.Game;
using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class FightWeaponAttackManager :
    MonoBehaviour
{
    private WeaponAttackProfileDamageRoller
        profileDamageRoller;

    private WeaponAttackDamageApplier
        damageApplier;

    public event Action<WeaponAttackRollResult>
        WeaponAttackRolled;

    public bool Initialize(
        WeaponAttackProfileDamageRoller
            profileDamageRoller,
        WeaponAttackDamageApplier
            damageApplier)
    {
        if (profileDamageRoller == null ||
            damageApplier == null)
        {
            return false;
        }

        this.profileDamageRoller =
            profileDamageRoller;

        this.damageApplier =
            damageApplier;

        return true;
    }

    public bool TryExecute(
        FightUnit attacker,
        FightUnit target)
    {
        return Execute(
            attacker,
            target) ==
            WeaponAttackExecutionResult.Success;
    }

    public WeaponAttackExecutionResult Execute(
        FightUnit attacker,
        FightUnit target)
    {
        if (attacker == null ||
            !attacker.IsAlive)
        {
            return WeaponAttackExecutionResult
                .InvalidAttacker;
        }

        if (target == null ||
            !target.IsAlive)
        {
            return WeaponAttackExecutionResult
                .InvalidTarget;
        }

        if (profileDamageRoller == null ||
            damageApplier == null)
        {
            return WeaponAttackExecutionResult
                .DamageApplicationFailed;
        }

        if (!FightWeaponAttackProfileResolver
                .TryResolve(
                    attacker.ActionSet,
                    out RolledWeaponProfile
                        weaponProfile))
        {
            return WeaponAttackExecutionResult
                .MissingWeaponProfile;
        }

        IReadOnlyList<
            WeaponAttackDamageLineResult>
            damageLines =
                profileDamageRoller.Roll(
                    weaponProfile);

        WeaponAttackRollResult attackResult =
            new WeaponAttackRollResult(
                attacker,
                target,
                damageLines);

        WeaponAttackApplyResult applyResult =
            damageApplier.Apply(
                attackResult);

        WeaponAttackExecutionResult executionResult =
    MapApplyResult(
        applyResult);

        if (executionResult !=
            WeaponAttackExecutionResult.Success)
        {
            return executionResult;
        }

        WeaponAttackRolled?.Invoke(
            attackResult);

        return WeaponAttackExecutionResult.Success;
    }

    private static WeaponAttackExecutionResult
    MapApplyResult(
        WeaponAttackApplyResult applyResult)
    {
        switch (applyResult)
        {
            case WeaponAttackApplyResult.Success:
                return WeaponAttackExecutionResult
                    .Success;

            case WeaponAttackApplyResult.InvalidAttack:
                return WeaponAttackExecutionResult
                    .DamageApplicationFailed;

            case WeaponAttackApplyResult.InvalidTarget:
                return WeaponAttackExecutionResult
                    .InvalidTarget;

            case WeaponAttackApplyResult.NoDamage:
                return WeaponAttackExecutionResult
                    .DamageApplicationFailed;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(applyResult),
                    applyResult,
                    null);
        }
    }
}