public sealed class WeaponAttackDamageLineResolver
{
    public int Resolve(
        WeaponAttackDamageLineResult damageLine)
    {
        if (damageLine == null)
        {
            return 0;
        }

        if (damageLine.Damage <= 0)
        {
            return 0;
        }

        return damageLine.Damage;
    }
}