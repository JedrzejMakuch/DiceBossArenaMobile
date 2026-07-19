namespace DiceBossArena.Game
{
    public interface IEquipmentAffixRandomSource
    {
        int Next(
            int minimumInclusive,
            int maximumExclusive);
    }
}