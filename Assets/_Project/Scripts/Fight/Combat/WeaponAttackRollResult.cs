using System;
using System.Collections.Generic;

public sealed class WeaponAttackRollResult
{
    private readonly IReadOnlyList<
        WeaponAttackDamageLineResult> damageLines;

    private readonly IReadOnlyList<
        WeaponAttackEffectLineResult> effectLines;

    private readonly int totalDamage;

    public FightUnit Attacker { get; }

    public FightUnit Target { get; }

    public IReadOnlyList<
        WeaponAttackDamageLineResult> DamageLines =>
            damageLines;

    public IReadOnlyList<
        WeaponAttackEffectLineResult> EffectLines =>
            effectLines;

    public int TotalDamage =>
        totalDamage;

    public WeaponAttackRollResult(
        FightUnit attacker,
        FightUnit target,
        IReadOnlyList<
            WeaponAttackDamageLineResult> damageLines,
        IReadOnlyList<
            WeaponAttackEffectLineResult> effectLines = null)
    {
        Attacker =
            attacker ??
            throw new ArgumentNullException(
                nameof(attacker));

        Target =
            target ??
            throw new ArgumentNullException(
                nameof(target));

        this.damageLines =
            CopyDamageLines(
                damageLines);

        this.effectLines =
            CopyEffectLines(
                effectLines);

        ValidateEffectLines();

        totalDamage =
            CalculateTotalDamage(
                this.damageLines);
    }

    private static IReadOnlyList<
        WeaponAttackDamageLineResult>
        CopyDamageLines(
            IReadOnlyList<
                WeaponAttackDamageLineResult> source)
    {
        if (source == null)
        {
            throw new ArgumentNullException(
                nameof(source));
        }

        if (source.Count == 0)
        {
            throw new ArgumentException(
                "Weapon attack result must contain " +
                "at least one damage line.",
                nameof(source));
        }

        WeaponAttackDamageLineResult[] copy =
            new WeaponAttackDamageLineResult[
                source.Count];

        for (int index = 0;
             index < source.Count;
             index++)
        {
            WeaponAttackDamageLineResult line =
                source[index];

            if (line == null)
            {
                throw new ArgumentException(
                    "Weapon attack result cannot " +
                    "contain null damage lines.",
                    nameof(source));
            }

            copy[index] =
                line;
        }

        return Array.AsReadOnly(
            copy);
    }

    private static IReadOnlyList<
        WeaponAttackEffectLineResult>
        CopyEffectLines(
            IReadOnlyList<
                WeaponAttackEffectLineResult> source)
    {
        if (source == null)
        {
            return Array.AsReadOnly(
                Array.Empty<
                    WeaponAttackEffectLineResult>());
        }

        WeaponAttackEffectLineResult[] copy =
            new WeaponAttackEffectLineResult[
                source.Count];

        for (int index = 0;
             index < source.Count;
             index++)
        {
            WeaponAttackEffectLineResult line =
                source[index];

            if (line == null)
            {
                throw new ArgumentException(
                    "Weapon attack result cannot " +
                    "contain null effect lines.",
                    nameof(source));
            }

            copy[index] =
                line;
        }

        return Array.AsReadOnly(
            copy);
    }

    private void ValidateEffectLines()
    {
        if (EffectLines.Count == 0)
        {
            return;
        }

        if (EffectLines.Count != DamageLines.Count)
        {
            throw new ArgumentException(
                "Effect line count must match " +
                "damage line count.",
                nameof(effectLines));
        }

        for (int index = 0;
             index < DamageLines.Count;
             index++)
        {
            if (!ReferenceEquals(
                    EffectLines[index].DamageLine,
                    DamageLines[index]))
            {
                throw new ArgumentException(
                    "Every effect line must reference " +
                    "the matching damage line.",
                    nameof(effectLines));
            }
        }
    }

    private static int CalculateTotalDamage(
        IReadOnlyList<
            WeaponAttackDamageLineResult> lines)
    {
        int result =
            0;

        for (int index = 0;
             index < lines.Count;
             index++)
        {
            result +=
                lines[index].Damage;
        }

        return result;
    }
}