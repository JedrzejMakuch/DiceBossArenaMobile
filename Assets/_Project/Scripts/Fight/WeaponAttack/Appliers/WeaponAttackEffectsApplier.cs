using System;
using System.Collections.Generic;

public sealed class WeaponAttackEffectsApplier
{
    private readonly IReadOnlyList<
        IWeaponAttackEffectApplier>
        appliers;

    public WeaponAttackEffectsApplier(
        IReadOnlyList<
            IWeaponAttackEffectApplier>
                appliers)
    {
        if (appliers == null)
        {
            throw new ArgumentNullException(
                nameof(appliers));
        }

        IWeaponAttackEffectApplier[]
            copiedAppliers =
                new IWeaponAttackEffectApplier[
                    appliers.Count];

        for (int index = 0;
             index < appliers.Count;
             index++)
        {
            IWeaponAttackEffectApplier applier =
                appliers[index];

            if (applier == null)
            {
                throw new ArgumentException(
                    "Effect appliers cannot contain null.",
                    nameof(appliers));
            }

            copiedAppliers[index] =
                applier;
        }

        this.appliers =
            copiedAppliers;
    }

    public void Apply(
        WeaponAttackRollResult attackResult)
    {
        if (attackResult == null)
        {
            throw new ArgumentNullException(
                nameof(attackResult));
        }

        foreach (
            IWeaponAttackEffectApplier applier
            in appliers)
        {
            applier.Apply(
                attackResult);
        }
    }
}