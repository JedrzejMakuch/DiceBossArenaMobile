using System;

namespace DiceBossArena.Game
{
    public readonly struct CharacterClassId :
        IEquatable<CharacterClassId>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public CharacterClassId(
            string value)
        {
            Value =
                value?.Trim() ?? string.Empty;
        }

        public bool Equals(
            CharacterClassId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterClassId other &&
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
    }
}