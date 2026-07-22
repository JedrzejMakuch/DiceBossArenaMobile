public sealed class WeaponAttackResistanceCalculator
{
    public int Calculate(
        int damage,
        int resistancePercent)
    {
        if (damage <= 0)
        {
            return 0;
        }

        int clampedResistance =
            UnityEngine.Mathf.Clamp(
                resistancePercent,
                0,
                100);

        float multiplier =
            1f - clampedResistance / 100f;

        return UnityEngine.Mathf.FloorToInt(
            damage * multiplier);
    }
}