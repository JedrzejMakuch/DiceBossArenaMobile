using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public static class FightStatCalculator
    {
        public static int Calculate(
            FightStatType statType,
            int baseValue,
            IEnumerable<FightStatModifier> modifiers)
        {
            if (modifiers == null)
            {
                return baseValue;
            }

            int totalFlat = 0;
            int totalPercent = 0;

            foreach (FightStatModifier modifier in modifiers)
            {
                if (modifier.StatType != statType)
                {
                    continue;
                }

                switch (modifier.ModifierType)
                {
                    case FightStatModifierType.Flat:
                        totalFlat += modifier.Value;
                        break;

                    case FightStatModifierType.Percent:
                        totalPercent += modifier.Value;
                        break;
                }
            }

            long valueAfterFlat =
                (long)baseValue + totalFlat;

            long scaledValue =
                valueAfterFlat * (100L + totalPercent);

            return (int)(scaledValue / 100L);
        }
    }
}