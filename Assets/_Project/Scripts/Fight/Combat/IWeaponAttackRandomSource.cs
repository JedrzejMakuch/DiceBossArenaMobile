public interface IWeaponAttackRandomSource
{
    int Next(
        int minimumInclusive,
        int maximumExclusive);
}