using System;

namespace DiceBossArena.Game
{
    public readonly struct CharacterItemInstanceId :
        IEquatable<CharacterItemInstanceId>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public CharacterItemInstanceId(
            string value)
        {
            Value =
                value?.Trim() ?? string.Empty;
        }

        public bool Equals(
            CharacterItemInstanceId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterItemInstanceId other &&
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