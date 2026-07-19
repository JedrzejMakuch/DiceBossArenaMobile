using System;

namespace DiceBossArena.Game
{
    public readonly struct EquipmentBaseTypeId :
        IEquatable<EquipmentBaseTypeId>
    {
        public string Value { get; }

        public bool IsValid =>
            !string.IsNullOrWhiteSpace(Value);

        public EquipmentBaseTypeId(
            string value)
        {
            Value =
                value?.Trim() ??
                string.Empty;
        }

        public bool Equals(
            EquipmentBaseTypeId other)
        {
            return string.Equals(
                Value,
                other.Value,
                StringComparison.Ordinal);
        }

        public override bool Equals(
            object obj)
        {
            return obj is EquipmentBaseTypeId other &&
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