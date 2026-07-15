using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterClassContentValidator
    {
        public void Validate(
            IReadOnlyList<ClassDefinition>
                classDefinitions,
            IReadOnlyList<SpecializationDefinition>
                specializationDefinitions)
        {
            ClassDefinitionCatalog classCatalog =
                new ClassDefinitionCatalog(
                    classDefinitions);

            _ =
                new SpecializationDefinitionCatalog(
                    specializationDefinitions);

            CharacterBuildComposer composer =
                new CharacterBuildComposer();

            ValidateClassBuilds(
                classDefinitions,
                composer);

            ValidateSpecializationBuilds(
                specializationDefinitions,
                classCatalog,
                composer);
        }

        private static void ValidateClassBuilds(
            IReadOnlyList<ClassDefinition>
                classDefinitions,
            CharacterBuildComposer composer)
        {
            if (classDefinitions == null)
            {
                return;
            }

            for (int i = 0;
                 i < classDefinitions.Count;
                 i++)
            {
                ClassDefinition classDefinition =
                    classDefinitions[i];

                CharacterBuildCompositionRequest request =
                    new CharacterBuildCompositionRequest(
                        classDefinition);

                composer.Compose(
                    request);
            }
        }

        private static void
            ValidateSpecializationBuilds(
                IReadOnlyList<
                    SpecializationDefinition>
                    specializationDefinitions,
                ClassDefinitionCatalog classCatalog,
                CharacterBuildComposer composer)
        {
            if (specializationDefinitions == null)
            {
                return;
            }

            for (int i = 0;
                 i < specializationDefinitions.Count;
                 i++)
            {
                SpecializationDefinition specialization =
                    specializationDefinitions[i];

                CharacterClassId requiredClassId =
                    specialization.RequiredClassId;

                if (!classCatalog.TryResolve(
                        requiredClassId,
                        out ClassDefinition
                            classDefinition))
                {
                    throw new InvalidOperationException(
                        $"Specialization " +
                        $"{specialization.SpecializationId.Value} " +
                        $"requires unknown class " +
                        $"{requiredClassId.Value}.");
                }

                if (!specialization.IsCompatibleWith(
                        classDefinition.ClassId))
                {
                    throw new InvalidOperationException(
                        $"Specialization " +
                        $"{specialization.SpecializationId.Value} " +
                        "is not compatible with its " +
                        "resolved class.");
                }

                List<CharacterBuildSkill>
                    representativeSelection =
                        CreateRepresentativeSelection(
                            classDefinition,
                            specialization);

                CharacterBuildCompositionRequest request =
                    new CharacterBuildCompositionRequest(
                        classDefinition,
                        specialization,
                        representativeSelection);

                composer.Compose(
                    request);
            }
        }

        private static List<CharacterBuildSkill>
            CreateRepresentativeSelection(
                ClassDefinition classDefinition,
                SpecializationDefinition specialization)
        {
            HashSet<string> startingSkillIds =
                new(
                    StringComparer.Ordinal);

            AddStartingSkillIds(
                startingSkillIds,
                classDefinition.StartingSkills);

            AddStartingSkillIds(
                startingSkillIds,
                specialization.StartingSkills);

            List<CharacterBuildSkill> result =
                new();

            for (int i = 0;
                 i < specialization
                     .SkillReplacements.Count;
                 i++)
            {
                SkillReplacementDefinition replacement =
                    specialization
                        .SkillReplacements[i];

                if (replacement == null ||
                    !replacement.IsValid)
                {
                    continue;
                }

                if (startingSkillIds.Contains(
                        replacement.SourceSkillId))
                {
                    continue;
                }

                result.Add(
                    new CharacterBuildSkill(
                        replacement.SourceSkillId,
                        1));
            }

            return result;
        }

        private static void AddStartingSkillIds(
            HashSet<string> destination,
            IReadOnlyList<
                CharacterSkillDefinitionEntry> entries)
        {
            if (entries == null)
            {
                return;
            }

            for (int i = 0;
                 i < entries.Count;
                 i++)
            {
                CharacterSkillDefinitionEntry entry =
                    entries[i];

                if (entry != null &&
                    entry.IsValid)
                {
                    destination.Add(
                        entry.SkillId);
                }
            }
        }
    }
}