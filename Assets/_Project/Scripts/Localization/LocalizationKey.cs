using System;

namespace DiceBossArena.Game
{
    public readonly struct LocalizationKey :
        IEquatable<LocalizationKey>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public LocalizationKey(
            string value)
        {
            Value =
                value?.Trim() ??
                string.Empty;
        }

        public bool Equals(
            LocalizationKey other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is LocalizationKey other &&
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
            LocalizationKey left,
            LocalizationKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            LocalizationKey left,
            LocalizationKey right)
        {
            return !left.Equals(right);
        }
    }
}