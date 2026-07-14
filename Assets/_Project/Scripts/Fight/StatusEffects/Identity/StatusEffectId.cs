using System;

namespace DiceBossArena.Game
{
    public readonly struct StatusEffectId :
        IEquatable<StatusEffectId>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public StatusEffectId(
            string value)
        {
            Value =
                value?.Trim() ??
                string.Empty;
        }

        public bool Equals(
            StatusEffectId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is StatusEffectId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return StringComparer.Ordinal.GetHashCode(
                Value ??
                string.Empty);
        }

        public override string ToString()
        {
            return Value ??
                   string.Empty;
        }

        public static bool operator ==(
            StatusEffectId left,
            StatusEffectId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            StatusEffectId left,
            StatusEffectId right)
        {
            return !left.Equals(right);
        }
    }
}