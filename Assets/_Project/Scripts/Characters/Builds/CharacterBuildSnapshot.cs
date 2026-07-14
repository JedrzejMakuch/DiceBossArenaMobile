using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public sealed class CharacterBuildSnapshot :
    IEquatable<CharacterBuildSnapshot>
    {
        private readonly ReadOnlyCollection<CharacterBuildSkill> skills;
        private readonly ReadOnlyCollection<FightStatModifier> statModifiers;
        private readonly ReadOnlyCollection<CharacterPassiveId> passiveIds;

        public CharacterClassId ClassId { get; }

        public CharacterSpecializationId SpecializationId { get; }

        public IReadOnlyList<CharacterBuildSkill> Skills =>
            skills;

        public IReadOnlyList<FightStatModifier> StatModifiers =>
            statModifiers;

        public EquipmentLoadoutSnapshot EquipmentLoadout { get; }
        public IReadOnlyList<CharacterPassiveId> PassiveIds =>
            passiveIds;

        public bool IsEmpty =>
            !ClassId.IsValid &&
            !SpecializationId.IsValid &&
            skills.Count == 0 &&
            statModifiers.Count == 0 &&
            EquipmentLoadout.Items.Count == 0 &&
            passiveIds.Count == 0;

        public static CharacterBuildSnapshot Empty =>
        new CharacterBuildSnapshot(
            new CharacterClassId(string.Empty),
            new CharacterSpecializationId(string.Empty),
            null,
            null,
            null,
            null);

        public CharacterBuildSnapshot(
            CharacterClassId classId,
            CharacterSpecializationId specializationId,
            IReadOnlyList<CharacterBuildSkill> skills,
            IReadOnlyList<FightStatModifier> statModifiers,
            EquipmentLoadoutSnapshot equipmentLoadout = null,
            IReadOnlyList<CharacterPassiveId> passiveIds = null)
        {
            ClassId = classId;
            SpecializationId = specializationId;

            this.skills =
                CopySkills(skills).AsReadOnly();

            this.statModifiers =
                CopyStatModifiers(statModifiers).AsReadOnly();

            EquipmentLoadout =
                equipmentLoadout ??
                new EquipmentLoadoutSnapshot(null);

            this.passiveIds =
                CopyPassiveIds(passiveIds).AsReadOnly();
        }

        private static List<CharacterBuildSkill> CopySkills(
    IReadOnlyList<CharacterBuildSkill> source)
        {
            List<CharacterBuildSkill> result =
                new();

            if (source == null)
            {
                return result;
            }

            HashSet<string> skillIds =
                new(StringComparer.Ordinal);

            for (int i = 0; i < source.Count; i++)
            {
                CharacterBuildSkill skill =
                    source[i];

                if (!skill.IsValid)
                {
                    throw new ArgumentException(
                        "Build contains an invalid skill.",
                        nameof(source));
                }

                if (!skillIds.Add(skill.SkillId))
                {
                    throw new ArgumentException(
                        $"Build contains duplicate skill id: " +
                        $"{skill.SkillId}.",
                        nameof(source));
                }

                result.Add(skill);
            }

            return result;
        }

        public CharacterBuildSnapshot Copy()
        {
            return new CharacterBuildSnapshot(
                ClassId,
                SpecializationId,
                skills,
                statModifiers,
                EquipmentLoadout,
                passiveIds);
        }

        private static List<FightStatModifier> CopyStatModifiers(
            IReadOnlyList<FightStatModifier> source)
        {
            List<FightStatModifier> result =
                new();

            if (source == null)
            {
                return result;
            }

            for (int i = 0; i < source.Count; i++)
            {
                result.Add(source[i]);
            }

            return result;
        }

        public bool Equals(
    CharacterBuildSnapshot other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (!EquipmentLoadout.Equals(
                other.EquipmentLoadout))
            {
                return false;
            }

            if (!ClassId.Equals(other.ClassId) ||
                !SpecializationId.Equals(
                    other.SpecializationId))
            {
                return false;
            }

            if (skills.Count != other.skills.Count ||
                statModifiers.Count !=
                other.statModifiers.Count ||
                passiveIds.Count !=
                other.passiveIds.Count)
            {
                return false;
            }

            for (int i = 0; i < passiveIds.Count; i++)
            {
                if (!passiveIds[i].Equals(
                        other.passiveIds[i]))
                {
                    return false;
                }
            }

            for (int i = 0; i < skills.Count; i++)
            {
                if (!skills[i].Equals(other.skills[i]))
                {
                    return false;
                }
            }

            for (int i = 0;
                 i < statModifiers.Count;
                 i++)
            {
                if (!statModifiers[i].Equals(
                        other.statModifiers[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(
            object obj)
        {
            return obj is CharacterBuildSnapshot other &&
                   Equals(other);
        }

        public override int GetHashCode()
        {
            HashCode hashCode =
                new();

            hashCode.Add(ClassId);
            hashCode.Add(SpecializationId);
            hashCode.Add(EquipmentLoadout);

            for (int i = 0; i < skills.Count; i++)
            {
                hashCode.Add(skills[i]);
            }

            for (int i = 0;
                 i < statModifiers.Count;
                 i++)
            {
                hashCode.Add(statModifiers[i]);
            }

            for (int i = 0; i < passiveIds.Count; i++)
            {
                hashCode.Add(passiveIds[i]);
            }

            return hashCode.ToHashCode();
        }

        private static List<CharacterPassiveId> CopyPassiveIds(
    IReadOnlyList<CharacterPassiveId> source)
        {
            List<CharacterPassiveId> result =
                new();

            if (source == null)
            {
                return result;
            }

            HashSet<string> ids =
                new(StringComparer.Ordinal);

            for (int i = 0; i < source.Count; i++)
            {
                CharacterPassiveId passiveId =
                    source[i];

                if (!passiveId.IsValid)
                {
                    throw new ArgumentException(
                        "Build contains an invalid passive id.",
                        nameof(source));
                }

                if (!ids.Add(passiveId.Value))
                {
                    throw new ArgumentException(
                        $"Build contains duplicate passive id: " +
                        $"{passiveId.Value}.",
                        nameof(source));
                }

                result.Add(passiveId);
            }

            return result;
        }
    }
}