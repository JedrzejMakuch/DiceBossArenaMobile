using System;

namespace DiceBossArena.Game
{
    public sealed class RolledWeaponAttackLine :
        IEquatable<RolledWeaponAttackLine>
    {
        public RolledWeaponAttackLine(
            WeaponAttackLineId lineId,
            WeaponAttackElement element,
            int minDamage,
            int maxDamage)
        {
            if (lineId == default)
            {
                throw new ArgumentException(
                    "Weapon attack line ID must be valid.",
                    nameof(lineId));
            }

            if (!Enum.IsDefined(
                    typeof(WeaponAttackElement),
                    element))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(element));
            }

            if (minDamage < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(minDamage));
            }

            if (maxDamage < minDamage)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxDamage));
            }

            LineId = lineId;
            Element = element;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        public WeaponAttackLineId LineId { get; }

        public WeaponAttackElement Element { get; }

        public int MinDamage { get; }

        public int MaxDamage { get; }

        public bool Equals(
            RolledWeaponAttackLine other)
        {
            return other != null &&
                   LineId == other.LineId &&
                   Element == other.Element &&
                   MinDamage == other.MinDamage &&
                   MaxDamage == other.MaxDamage;
        }

        public override bool Equals(
            object obj)
        {
            return Equals(
                obj as RolledWeaponAttackLine);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = LineId.GetHashCode();
                hash = (hash * 397) ^ (int)Element;
                hash = (hash * 397) ^ MinDamage;
                hash = (hash * 397) ^ MaxDamage;
                return hash;
            }
        }
    }
}