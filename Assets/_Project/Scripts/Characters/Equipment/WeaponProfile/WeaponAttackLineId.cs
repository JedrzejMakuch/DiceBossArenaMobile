using System;

namespace DiceBossArena.Game
{
    [Serializable]
    public readonly struct WeaponAttackLineId :
        IEquatable<WeaponAttackLineId>
    {
        public WeaponAttackLineId(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Weapon attack line ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public string Value { get; }

        public bool Equals(
            WeaponAttackLineId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is WeaponAttackLineId other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            return Value == null
                ? 0
                : StringComparer.Ordinal.GetHashCode(
                    Value);
        }

        public override string ToString()
        {
            return Value;
        }

        public static bool operator ==(
            WeaponAttackLineId left,
            WeaponAttackLineId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            WeaponAttackLineId left,
            WeaponAttackLineId right)
        {
            return !left.Equals(right);
        }
    }
}