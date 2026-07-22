using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class RolledWeaponAttackLine :
        IEquatable<RolledWeaponAttackLine>
    {
        private readonly List<WeaponAttackEffectDefinition>
            effects;

        public RolledWeaponAttackLine(
            WeaponAttackLineId lineId,
            WeaponAttackElement element,
            int minDamage,
            int maxDamage,
            IEnumerable<WeaponAttackEffectDefinition>
                newEffects = null)
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

            LineId =
                lineId;

            Element =
                element;

            MinDamage =
                minDamage;

            MaxDamage =
                maxDamage;

            effects =
                newEffects == null
                    ? new List<WeaponAttackEffectDefinition>()
                    : new List<WeaponAttackEffectDefinition>(
                        newEffects);
        }

        public WeaponAttackLineId LineId { get; }

        public WeaponAttackElement Element { get; }

        public int MinDamage { get; }

        public int MaxDamage { get; }

        public IReadOnlyList<WeaponAttackEffectDefinition>
            Effects =>
                effects;

        public bool Equals(
            RolledWeaponAttackLine other)
        {
            if (other == null ||
                LineId != other.LineId ||
                Element != other.Element ||
                MinDamage != other.MinDamage ||
                MaxDamage != other.MaxDamage ||
                Effects.Count != other.Effects.Count)
            {
                return false;
            }

            for (int index = 0;
                 index < Effects.Count;
                 index++)
            {
                if (!ReferenceEquals(
                        Effects[index],
                        other.Effects[index]))
                {
                    return false;
                }
            }

            return true;
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
                int hash =
                    LineId.GetHashCode();

                hash =
                    (hash * 397) ^
                    (int)Element;

                hash =
                    (hash * 397) ^
                    MinDamage;

                hash =
                    (hash * 397) ^
                    MaxDamage;

                for (int index = 0;
                     index < Effects.Count;
                     index++)
                {
                    hash =
                        (hash * 397) ^
                        (Effects[index]?.GetHashCode() ?? 0);
                }

                return hash;
            }
        }
    }
}