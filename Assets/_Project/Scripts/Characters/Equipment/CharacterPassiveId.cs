using System;

namespace DiceBossArena.Game
{
    public readonly struct CharacterPassiveId :
        IEquatable<CharacterPassiveId>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public CharacterPassiveId(
            string value)
        {
            Value =
                value?.Trim() ?? string.Empty;
        }

        public bool Equals(
            CharacterPassiveId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterPassiveId other &&
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