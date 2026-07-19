using System;

namespace DiceBossArena.Game
{
    [Serializable]
    public readonly struct EquipmentAffixId :
        IEquatable<EquipmentAffixId>
    {
        public EquipmentAffixId(
            string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException(
                    "Equipment affix ID cannot be empty.",
                    nameof(value));
            }

            Value = value;
        }

        public string Value { get; }

        public bool Equals(
            EquipmentAffixId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is EquipmentAffixId other &&
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
            EquipmentAffixId left,
            EquipmentAffixId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(
            EquipmentAffixId left,
            EquipmentAffixId right)
        {
            return !left.Equals(right);
        }
    }
}