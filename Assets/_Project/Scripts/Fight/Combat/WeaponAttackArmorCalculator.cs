public sealed class WeaponAttackArmorCalculator
{
    public int Calculate(
        int damage,
        int armor)
    {
        if (damage <= 0)
        {
            return 0;
        }

        int effectiveArmor =
            UnityEngine.Mathf.Max(
                0,
                armor);

        return UnityEngine.Mathf.Max(
            0,
            damage - effectiveArmor);
    }
}