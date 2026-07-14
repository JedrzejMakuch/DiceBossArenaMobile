using System;

namespace DiceBossArena.Game
{
    public readonly struct FightStatModifier :
        IEquatable<FightStatModifier>
    {
        public FightStatType StatType { get; }

        public FightStatModifierType ModifierType { get; }

        public int Value { get; }

        public FightStatModifier(
            FightStatType statType,
            FightStatModifierType modifierType,
            int value)
        {
            StatType = statType;
            ModifierType = modifierType;
            Value = value;
        }

        public bool Equals(
            FightStatModifier other)
        {
            return StatType == other.StatType &&
                   ModifierType == other.ModifierType &&
                   Value == other.Value;
        }

        public override bool Equals(
            object obj)
        {
            return obj is FightStatModifier other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                StatType,
                ModifierType,
                Value);
        }

        public static bool operator ==(
            FightStatModifier left,
            FightStatModifier right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            FightStatModifier left,
            FightStatModifier right)
        {
            return !left.Equals(right);
        }
    }
}