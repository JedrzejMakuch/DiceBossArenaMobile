public sealed class WeaponAttackDamageApplier
{
    private readonly WeaponAttackDamageLineResolver
        damageLineResolver;

    public WeaponAttackDamageApplier()
    {
        damageLineResolver =
            new WeaponAttackDamageLineResolver();
    }

    public WeaponAttackApplyResult Apply(
        WeaponAttackRollResult attackResult)
    {
        if (attackResult == null)
        {
            return WeaponAttackApplyResult.InvalidAttack;
        }

        FightUnit target =
            attackResult.Target;

        if (target == null ||
            !target.IsAlive)
        {
            return WeaponAttackApplyResult.InvalidTarget;
        }

        int totalDamage = 0;

        for (int i = 0;
             i < attackResult.DamageLines.Count;
             i++)
        {
            totalDamage +=
                damageLineResolver.Resolve(
                    attackResult.DamageLines[i],
                    target.Stats);
        }

        if (totalDamage <= 0)
        {
            return WeaponAttackApplyResult.NoDamage;
        }

        target.TakeDamage(
            totalDamage);

        return WeaponAttackApplyResult.Success;
    }
}