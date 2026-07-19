using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class
        EquipmentBaseTypeStatModifierResolver
    {
        public IReadOnlyList<FightStatModifier> Resolve(
            EquipmentBaseTypeDefinition definition)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            List<FightStatModifier> result =
                new();

            for (int i = 0;
                 i < definition.StatModifiers.Count;
                 i++)
            {
                CharacterStatModifierDefinition
                    modifierDefinition =
                        definition.StatModifiers[i];

                if (modifierDefinition == null)
                {
                    throw new InvalidOperationException(
                        $"Equipment base type " +
                        $"{definition.BaseTypeId.Value} " +
                        $"contains a null stat modifier.");
                }

                result.Add(
                    modifierDefinition.CreateModifier());
            }

            return result;
        }
    }
}