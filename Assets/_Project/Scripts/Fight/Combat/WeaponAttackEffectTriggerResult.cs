using DiceBossArena.Game;

public sealed class WeaponAttackEffectTriggerResult
{
    public WeaponAttackEffectDefinition Definition { get; }

    public bool IsTriggered { get; }

    public WeaponAttackEffectTriggerResult(
        WeaponAttackEffectDefinition definition,
        bool isTriggered)
    {
        Definition =
            definition;

        IsTriggered =
            isTriggered;
    }

    public static WeaponAttackEffectTriggerResult NotTriggered(
        WeaponAttackEffectDefinition definition)
    {
        return new WeaponAttackEffectTriggerResult(
            definition,
            false);
    }

    public static WeaponAttackEffectTriggerResult Triggered(
        WeaponAttackEffectDefinition definition)
    {
        return new WeaponAttackEffectTriggerResult(
            definition,
            true);
    }
}