using System;

namespace DiceBossArena.Game
{
    public readonly struct CharacterSpecializationId :
        IEquatable<CharacterSpecializationId>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public CharacterSpecializationId(
            string value)
        {
            Value =
                value?.Trim() ?? string.Empty;
        }

        public bool Equals(
            CharacterSpecializationId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterSpecializationId other &&
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