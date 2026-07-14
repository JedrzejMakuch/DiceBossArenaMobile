using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public sealed class ResolvedCharacterBuild
    {
        private readonly ReadOnlyCollection<
            UnitStartingSkill> skills;

        private readonly ReadOnlyCollection<
            FightStatModifier> statModifiers;

        public CharacterClassId ClassId { get; }

        public CharacterSpecializationId SpecializationId { get; }

        public IReadOnlyList<UnitStartingSkill> Skills =>
            skills;

        public IReadOnlyList<FightStatModifier> StatModifiers =>
            statModifiers;

        public EquipmentLoadoutSnapshot EquipmentLoadout { get; }

        public IReadOnlyList<CharacterPassiveId> PassiveIds { get; }

        public ResolvedCharacterBuild(
            CharacterClassId classId,
            CharacterSpecializationId specializationId,
            IReadOnlyList<UnitStartingSkill> skills,
            IReadOnlyList<FightStatModifier> statModifiers,
            EquipmentLoadoutSnapshot equipmentLoadout,
            IReadOnlyList<CharacterPassiveId> passiveIds)
        {
            ClassId = classId;
            SpecializationId = specializationId;

            this.skills =
                CopySkills(skills).AsReadOnly();

            this.statModifiers =
                CopyModifiers(statModifiers).AsReadOnly();

            EquipmentLoadout =
                equipmentLoadout ??
                new EquipmentLoadoutSnapshot(null);

            PassiveIds =
                CopyPassives(passiveIds).AsReadOnly();
        }

        private static List<UnitStartingSkill> CopySkills(
            IReadOnlyList<UnitStartingSkill> source)
        {
            List<UnitStartingSkill> result =
                new();

            if (source == null)
            {
                return result;
            }

            for (int i = 0; i < source.Count; i++)
            {
                UnitStartingSkill skill =
                    source[i];

                result.Add(
                    new UnitStartingSkill(
                        skill.Definition,
                        skill.Level));
            }

            return result;
        }

        private static List<FightStatModifier> CopyModifiers(
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

        private static List<CharacterPassiveId> CopyPassives(
            IReadOnlyList<CharacterPassiveId> source)
        {
            List<CharacterPassiveId> result =
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
    }
}