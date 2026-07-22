using DiceBossArena.Game;

public sealed class WeaponAttackResistanceResolver
    : IWeaponAttackDamageModifier
{
    private readonly WeaponAttackResistanceStatResolver
        resistanceStatResolver =
            new();

    private readonly WeaponAttackResistanceCalculator
        resistanceCalculator =
            new();

    public int Resolve(
        WeaponAttackDamageLineResult damageLine,
        FightUnitStats targetStats,
        int damage)
    {
        if (damageLine == null)
        {
            return 0;
        }

        if (damage <= 0)
        {
            return 0;
        }

        if (targetStats == null)
        {
            return damage;
        }

        if (!resistanceStatResolver.TryResolve(
                damageLine.Element,
                out FightStatType resistanceStatType))
        {
            return damage;
        }

        int resistancePercent =
            targetStats.GetFinalValue(
                resistanceStatType);

        return resistanceCalculator.Calculate(
            damage,
            resistancePercent);
    }
}