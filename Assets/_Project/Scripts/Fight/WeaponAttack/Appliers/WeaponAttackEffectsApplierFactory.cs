public sealed class WeaponAttackEffectsApplierFactory
{
    public WeaponAttackEffectsApplier Create()
    {
        WeaponAttackLifeStealCalculator
            lifeStealCalculator =
                new WeaponAttackLifeStealCalculator();

        WeaponAttackLifeStealApplier
            lifeStealApplier =
                new WeaponAttackLifeStealApplier(
                    lifeStealCalculator);

        return new WeaponAttackEffectsApplier(
            new IWeaponAttackEffectApplier[]
            {
                lifeStealApplier
            });
    }
}