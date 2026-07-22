using System;
using System.Collections.Generic;

public sealed class WeaponAttackRollResult
{
    private readonly IReadOnlyList<
        WeaponAttackDamageLineResult> damageLines;

    public FightUnit Attacker { get; }

    public FightUnit Target { get; }

    public IReadOnlyList<
        WeaponAttackDamageLineResult> DamageLines =>
            damageLines;

    private readonly int totalDamage;
    public int TotalDamage =>
    totalDamage;

    public WeaponAttackRollResult(
        FightUnit attacker,
        FightUnit target,
        IReadOnlyList<
            WeaponAttackDamageLineResult> damageLines)
    {
        Attacker =
            attacker ??
            throw new ArgumentNullException(
                nameof(attacker));

        Target =
            target ??
            throw new ArgumentNullException(
                nameof(target));

        if (damageLines == null)
        {
            throw new ArgumentNullException(
                nameof(damageLines));
        }

        if (damageLines.Count == 0)
        {
            throw new ArgumentException(
                "Weapon attack result must contain " +
                "at least one damage line.",
                nameof(damageLines));
        }

        WeaponAttackDamageLineResult[] copiedLines =
            new WeaponAttackDamageLineResult[
                damageLines.Count];

        this.damageLines =
    Array.AsReadOnly(copiedLines);

        for (int i = 0;
             i < damageLines.Count;
             i++)
        {
            WeaponAttackDamageLineResult line =
                damageLines[i];

            if (line == null)
            {
                throw new ArgumentException(
                    "Weapon attack result cannot " +
                    "contain null damage lines.",
                    nameof(damageLines));
            }

            copiedLines[i] = line;
        }

        this.damageLines =
            Array.AsReadOnly(copiedLines);

        int calculatedTotalDamage = 0;

        for (int i = 0;
             i < copiedLines.Length;
             i++)
        {
            calculatedTotalDamage +=
                copiedLines[i].Damage;
        }

        totalDamage =
            calculatedTotalDamage;
    }
}