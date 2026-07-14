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

        public event Action<FightStatType> StatChanged;

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

            StatChanged?.Invoke(
                modifier.StatType);
        }

        public bool RemoveModifier(
    FightStatModifier modifier)
        {
            bool removed =
                modifiers.Remove(modifier);

            if (removed)
            {
                StatChanged?.Invoke(
                    modifier.StatType);
            }

            return removed;
        }

        public void ClearModifiers()
        {
            if (modifiers.Count == 0)
            {
                return;
            }

            HashSet<FightStatType> changedStats =
                new();

            foreach (FightStatModifier modifier in modifiers)
            {
                changedStats.Add(
                    modifier.StatType);
            }

            modifiers.Clear();

            foreach (FightStatType statType in changedStats)
            {
                StatChanged?.Invoke(
                    statType);
            }
        }
    }
}