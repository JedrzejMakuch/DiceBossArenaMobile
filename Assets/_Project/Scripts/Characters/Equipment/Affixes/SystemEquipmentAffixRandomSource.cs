using System;

namespace DiceBossArena.Game
{
    public sealed class SystemEquipmentAffixRandomSource :
        IEquipmentAffixRandomSource
    {
        private readonly Random random;

        public SystemEquipmentAffixRandomSource()
            : this(new Random())
        {
        }

        public SystemEquipmentAffixRandomSource(
            int seed)
            : this(new Random(seed))
        {
        }

        private SystemEquipmentAffixRandomSource(
            Random newRandom)
        {
            random =
                newRandom ??
                throw new ArgumentNullException(
                    nameof(newRandom));
        }

        public int Next(
            int minimumInclusive,
            int maximumExclusive)
        {
            return random.Next(
                minimumInclusive,
                maximumExclusive);
        }
    }
}