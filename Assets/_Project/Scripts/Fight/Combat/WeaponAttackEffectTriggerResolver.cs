using System;

public sealed class WeaponAttackEffectTriggerResolver
{
    private readonly IWeaponAttackRandomSource randomSource;

    public WeaponAttackEffectTriggerResolver(
        IWeaponAttackRandomSource newRandomSource)
    {
        randomSource =
            newRandomSource ??
            throw new ArgumentNullException(
                nameof(newRandomSource));
    }

    public WeaponAttackEffectTriggerResult Resolve(
    DiceBossArena.Game.WeaponAttackEffectDefinition definition)
    {
        if (definition == null)
        {
            return WeaponAttackEffectTriggerResult.NotTriggered(
                null);
        }

        int triggerChance =
            definition.TriggerChancePercent;

        if (triggerChance <= 0)
        {
            return WeaponAttackEffectTriggerResult.NotTriggered(
                definition);
        }

        if (triggerChance >= 100)
        {
            return WeaponAttackEffectTriggerResult.Triggered(
                definition);
        }

        int roll =
            randomSource.Next(
                0,
                100);

        if (roll < triggerChance)
        {
            return WeaponAttackEffectTriggerResult.Triggered(
                definition);
        }

        return WeaponAttackEffectTriggerResult.NotTriggered(
            definition);
    }
}