using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class RolledWeaponProfile :
    IEquatable<RolledWeaponProfile>
    {
        private readonly RolledWeaponAttackLine[] lines;

        public RolledWeaponProfile(
            IEnumerable<RolledWeaponAttackLine> newLines)
        {
            if (newLines == null)
            {
                throw new ArgumentNullException(
                    nameof(newLines));
            }

            List<RolledWeaponAttackLine> copiedLines =
                new List<RolledWeaponAttackLine>();

            HashSet<WeaponAttackLineId> lineIds =
                new HashSet<WeaponAttackLineId>();

            foreach (RolledWeaponAttackLine line
                     in newLines)
            {
                if (line == null)
                {
                    throw new ArgumentException(
                        "Weapon profile cannot contain null lines.",
                        nameof(newLines));
                }

                if (!lineIds.Add(line.LineId))
                {
                    throw new ArgumentException(
                        "Weapon profile cannot contain " +
                        "duplicate line IDs.",
                        nameof(newLines));
                }

                copiedLines.Add(line);
            }

            if (copiedLines.Count == 0)
            {
                throw new ArgumentException(
                    "Weapon profile must contain at least one line.",
                    nameof(newLines));
            }

            lines = copiedLines.ToArray();
        }

        public bool Equals(
    RolledWeaponProfile other)
        {
            if (other == null ||
                Lines.Count != other.Lines.Count)
            {
                return false;
            }

            for (int index = 0;
                 index < Lines.Count;
                 index++)
            {
                if (!Lines[index].Equals(
                        other.Lines[index]))
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
                obj as RolledWeaponProfile);
        }

        public override int GetHashCode()
        {
            HashCode hashCode =
                new HashCode();

            for (int index = 0;
                 index < Lines.Count;
                 index++)
            {
                hashCode.Add(
                    Lines[index]);
            }

            return hashCode.ToHashCode();
        }

        public IReadOnlyList<RolledWeaponAttackLine> Lines =>
            lines;
    }
}