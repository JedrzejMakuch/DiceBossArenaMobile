using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterBuildCompositionRequest
    {
        private readonly ReadOnlyCollection<
            CharacterBuildSkill> selectedSkills;

        public ClassDefinition ClassDefinition
        {
            get;
        }

        public SpecializationDefinition
            SpecializationDefinition
        {
            get;
        }

        public IReadOnlyList<CharacterBuildSkill>
            SelectedSkills =>
                selectedSkills;

        public CharacterBuildCompositionRequest(
            ClassDefinition classDefinition,
            SpecializationDefinition
                specializationDefinition = null,
            IReadOnlyList<CharacterBuildSkill>
                selectedSkills = null)
        {
            ClassDefinition =
                classDefinition ??
                throw new ArgumentNullException(
                    nameof(classDefinition));

            SpecializationDefinition =
                specializationDefinition;

            this.selectedSkills =
                CopySkills(
                    selectedSkills)
                    .AsReadOnly();
        }

        private static List<CharacterBuildSkill>
            CopySkills(
                IReadOnlyList<CharacterBuildSkill>
                    source)
        {
            List<CharacterBuildSkill> result =
                new();

            if (source == null)
            {
                return result;
            }

            for (int i = 0;
                 i < source.Count;
                 i++)
            {
                result.Add(
                    source[i]);
            }

            return result;
        }
    }
}