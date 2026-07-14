using System;

namespace DiceBossArena.Game
{
    public readonly struct FightUnitDefinitionId :
        IEquatable<FightUnitDefinitionId>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public FightUnitDefinitionId(
            string value)
        {
            Value =
                value?.Trim() ??
                string.Empty;
        }

        public bool Equals(
            FightUnitDefinitionId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is FightUnitDefinitionId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value ?? string.Empty);
        }

        public override string ToString()
        {
            return Value ?? string.Empty;
        }

        public static bool operator ==(
            FightUnitDefinitionId left,
            FightUnitDefinitionId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            FightUnitDefinitionId left,
            FightUnitDefinitionId right)
        {
            return !left.Equals(right);
        }
    }
}