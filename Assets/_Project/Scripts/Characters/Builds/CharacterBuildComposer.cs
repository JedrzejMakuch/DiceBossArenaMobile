using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class CharacterBuildComposer
    {
        private readonly EquipmentStatModifierResolver
    equipmentStatModifierResolver;

        public CharacterBuildComposer(
    EquipmentStatModifierResolver
        equipmentStatModifierResolver = null)
        {
            this.equipmentStatModifierResolver =
                equipmentStatModifierResolver;
        }

        public CharacterBuildSnapshot Compose(
            CharacterBuildCompositionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(
                    nameof(request));
            }

            ClassDefinition classDefinition =
                request.ClassDefinition;

            CharacterClassId classId =
                classDefinition.ClassId;

            if (!classId.IsValid)
            {
                throw new InvalidOperationException(
                    "Class definition must have a valid id.");
            }

            SpecializationDefinition specialization =
                request.SpecializationDefinition;

            CharacterSpecializationId specializationId =
                new CharacterSpecializationId(
                    string.Empty);

            if (specialization != null)
            {
                specializationId =
                    specialization.SpecializationId;

                if (!specializationId.IsValid)
                {
                    throw new InvalidOperationException(
                        "Specialization definition must " +
                        "have a valid id.");
                }

                if (!specialization.IsCompatibleWith(
                        classId))
                {
                    throw new InvalidOperationException(
                        $"Specialization " +
                        $"{specializationId.Value} is not " +
                        $"compatible with class " +
                        $"{classId.Value}.");
                }
            }

            List<CharacterBuildSkill> skills = CreateSkills(classDefinition.StartingSkills);

            if (specialization != null)
            {
                skills.AddRange(
                    CreateSkills(
                        specialization.StartingSkills));
            }

            HashSet<string> availableSkillIds =
                CreateSkillPool(
                    classDefinition.SkillPoolIds);

            if (specialization != null)
            {
                AddToSkillPool(
                    availableSkillIds,
                    specialization.SkillPoolIds);
            }

            AddSelectedSkills(
                skills,
                request.SelectedSkills,
                availableSkillIds);

            if (specialization != null)
            {
                ApplySkillReplacements(
                    skills,
                    specialization.SkillReplacements);
            }

            List<CharacterPassiveId> passiveIds =
                CreatePassiveIds(
                    classDefinition.Passives);

            if (specialization != null)
            {
                passiveIds.AddRange(
                    CreatePassiveIds(
                        specialization.Passives));
            }

            List<FightStatModifier> statModifiers = CreateStatModifiers(classDefinition.StatModifiers);

            if (specialization != null)
            {
                statModifiers.AddRange(
                    CreateStatModifiers(
                        specialization.StatModifiers));
            }

            if (equipmentStatModifierResolver != null)
            {
                statModifiers.AddRange(
                    equipmentStatModifierResolver.Resolve(
                        request.EquipmentLoadout));
            }

            return new CharacterBuildSnapshot(
                classId,
                specializationId,
                skills:
                    skills,
                statModifiers:
                    statModifiers,
                equipmentLoadout:
    request.EquipmentLoadout,
                passiveIds:
                    passiveIds);
                    }

        private static List<CharacterBuildSkill>
    CreateSkills(
        IReadOnlyList<
            CharacterSkillDefinitionEntry>
            entries)
        {
            List<CharacterBuildSkill> result =
                new();

            if (entries == null)
            {
                return result;
            }

            for (int i = 0;
                 i < entries.Count;
                 i++)
            {
                CharacterSkillDefinitionEntry entry =
                    entries[i];

                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Skill definition entry " +
                        "cannot be null.");
                }

                if (!entry.IsValid)
                {
                    throw new InvalidOperationException(
                        "Skill definition entry " +
                        "must have a valid skill id " +
                        "and level.");
                }

                result.Add(
                    entry.CreateBuildSkill());
            }

            return result;
        }

        private static List<CharacterPassiveId>
    CreatePassiveIds(
        IReadOnlyList<
            CharacterPassiveDefinitionEntry>
            entries)
        {
            List<CharacterPassiveId> result =
                new();

            if (entries == null)
            {
                return result;
            }

            for (int i = 0;
                 i < entries.Count;
                 i++)
            {
                CharacterPassiveDefinitionEntry entry =
                    entries[i];

                if (entry == null)
                {
                    throw new InvalidOperationException(
                        "Passive definition entry " +
                        "cannot be null.");
                }

                if (!entry.IsValid)
                {
                    throw new InvalidOperationException(
                        "Passive definition entry " +
                        "must have a valid id.");
                }

                result.Add(
                    entry.CreatePassiveId());
            }

            return result;
        }

        private static List<FightStatModifier>
    CreateStatModifiers(
        IReadOnlyList<
            CharacterStatModifierDefinition>
            definitions)
        {
            List<FightStatModifier> result =
                new();

            if (definitions == null)
            {
                return result;
            }

            for (int i = 0;
                 i < definitions.Count;
                 i++)
            {
                CharacterStatModifierDefinition
                    definition =
                        definitions[i];

                if (definition == null)
                {
                    throw new InvalidOperationException(
                        "Stat modifier definition " +
                        "cannot be null.");
                }

                result.Add(
                    definition.CreateModifier());
            }

            return result;
        }

        private static HashSet<string>
    CreateSkillPool(
        IReadOnlyList<string> skillIds)
        {
            HashSet<string> result =
                new(
                    StringComparer.Ordinal);

            AddToSkillPool(
                result,
                skillIds);

            return result;
        }

        private static void AddToSkillPool(
            HashSet<string> destination,
            IReadOnlyList<string> skillIds)
        {
            if (skillIds == null)
            {
                return;
            }

            for (int i = 0;
                 i < skillIds.Count;
                 i++)
            {
                string skillId =
                    skillIds[i]?.Trim() ??
                    string.Empty;

                if (string.IsNullOrWhiteSpace(
                        skillId))
                {
                    throw new InvalidOperationException(
                        "Skill pool contains an " +
                        "invalid skill id.");
                }

                destination.Add(
                    skillId);
            }
        }

        private static void AddSelectedSkills(
           List<CharacterBuildSkill> destination,
           IReadOnlyList<CharacterBuildSkill>
               selectedSkills,
           HashSet<string>
               availableSkillIds)
        {
            if (selectedSkills == null)
            {
                return;
            }

            for (int i = 0;
                 i < selectedSkills.Count;
                 i++)
            {
                CharacterBuildSkill selectedSkill =
                    selectedSkills[i];

                if (!selectedSkill.IsValid)
                {
                    throw new InvalidOperationException(
                        "Selected skill must have " +
                        "a valid id.");
                }

                if (availableSkillIds == null ||
                    !availableSkillIds.Contains(
                        selectedSkill.SkillId))
                {
                    throw new InvalidOperationException(
                        $"Selected skill " +
                        $"{selectedSkill.SkillId} is not " +
                        "available for this build.");
                }

                destination.Add(
                    selectedSkill);
            }
        }

        private static void ApplySkillReplacements(
    List<CharacterBuildSkill> skills,
    IReadOnlyList<
        SkillReplacementDefinition>
        replacements)
        {
            if (replacements == null ||
                replacements.Count == 0)
            {
                return;
            }

            Dictionary<string, string>
                replacementIdsBySource =
                    new(
                        StringComparer.Ordinal);

            for (int i = 0;
                 i < replacements.Count;
                 i++)
            {
                SkillReplacementDefinition replacement =
                    replacements[i];

                if (replacement == null)
                {
                    throw new InvalidOperationException(
                        "Skill replacement definition " +
                        "cannot be null.");
                }

                if (!replacement.IsValid)
                {
                    throw new InvalidOperationException(
                        "Skill replacement definition " +
                        "must contain different, valid ids.");
                }

                if (!replacementIdsBySource.TryAdd(
                        replacement.SourceSkillId,
                        replacement.ReplacementSkillId))
                {
                    throw new InvalidOperationException(
                        $"Duplicate replacement for skill " +
                        $"{replacement.SourceSkillId}.");
                }
            }

            HashSet<string> matchedSourceIds =
                new(
                    StringComparer.Ordinal);

            for (int i = 0;
                 i < skills.Count;
                 i++)
            {
                CharacterBuildSkill skill =
                    skills[i];

                if (!replacementIdsBySource.TryGetValue(
                        skill.SkillId,
                        out string replacementSkillId))
                {
                    continue;
                }

                skills[i] =
                    new CharacterBuildSkill(
                        replacementSkillId,
                        skill.Level);

                matchedSourceIds.Add(
                    skill.SkillId);
            }

            if (matchedSourceIds.Count !=
                replacementIdsBySource.Count)
            {
                throw new InvalidOperationException(
                    "At least one skill replacement " +
                    "does not match a build skill.");
            }
        }
    }
}