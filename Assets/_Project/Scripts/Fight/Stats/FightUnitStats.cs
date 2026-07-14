using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class FightUnitStats
    {
        private readonly Dictionary<FightStatType, int> baseValues =
            new();

        private readonly List<FightStatModifier> modifiers =
            new();

        public FightUnitStats(
            IReadOnlyDictionary<FightStatType, int> initialBaseValues)
        {
            if (initialBaseValues == null)
            {
                throw new ArgumentNullException(
                    nameof(initialBaseValues));
            }

            foreach (
                KeyValuePair<FightStatType, int> entry
                in initialBaseValues)
            {
                baseValues[entry.Key] = entry.Value;
            }
        }

        public int GetBaseValue(
            FightStatType statType)
        {
            return baseValues.TryGetValue(
                statType,
                out int value)
                ? value
                : 0;
        }

        public int GetFinalValue(
            FightStatType statType)
        {
            return FightStatCalculator.Calculate(
                statType,
                GetBaseValue(statType),
                modifiers);
        }

        public void AddModifier(
            FightStatModifier modifier)
        {
            modifiers.Add(modifier);
        }

        public bool RemoveModifier(
            FightStatModifier modifier)
        {
            return modifiers.Remove(modifier);
        }

        public void ClearModifiers()
        {
            modifiers.Clear();
        }
    }
}