using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class CharacterItemBuildStatModifierResolver
    {
        private readonly ItemDefinitionCatalog itemCatalog;
        private readonly EquipmentBaseTypeStatModifierResolver
            baseTypeResolver;
        private readonly CharacterItemStatModifierResolver
            rolledAffixResolver;

        public CharacterItemBuildStatModifierResolver(
            ItemDefinitionCatalog itemCatalog)
        {
            this.itemCatalog =
                itemCatalog ??
                throw new ArgumentNullException(
                    nameof(itemCatalog));

            baseTypeResolver =
                new EquipmentBaseTypeStatModifierResolver();

            rolledAffixResolver =
                new CharacterItemStatModifierResolver();
        }

        public IReadOnlyList<FightStatModifier> Resolve(
            CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Character item instance must be valid.",
                    nameof(item));
            }

            if (!itemCatalog.TryResolve(
                    item.ItemId,
                    out ItemDefinition definition))
            {
                throw new InvalidOperationException(
                    $"Could not resolve item definition: " +
                    $"{item.ItemId.Value}.");
            }

            ValidateBaseType(
                item,
                definition);

            List<FightStatModifier> result =
                new List<FightStatModifier>();

            AddBaseTypeModifiers(
                result,
                definition);

            AddDefinitionModifiers(
                result,
                definition);

            AddRolledAffixModifiers(
                result,
                item);

            return result;
        }

        private static void ValidateBaseType(
            CharacterItemInstance item,
            ItemDefinition definition)
        {
            EquipmentBaseTypeDefinition baseType =
                definition.BaseType;

            if (baseType == null)
            {
                if (item.BaseTypeId.IsValid)
                {
                    throw new InvalidOperationException(
                        $"Item instance {item.InstanceId.Value} " +
                        $"contains base type " +
                        $"{item.BaseTypeId.Value}, but item " +
                        $"{definition.ItemId.Value} has no " +
                        $"base type definition.");
                }

                return;
            }

            if (!item.BaseTypeId.Equals(
                    baseType.BaseTypeId))
            {
                throw new InvalidOperationException(
                    $"Item instance {item.InstanceId.Value} " +
                    $"uses base type {item.BaseTypeId.Value}, " +
                    $"but item definition " +
                    $"{definition.ItemId.Value} uses " +
                    $"{baseType.BaseTypeId.Value}.");
            }
        }

        private void AddBaseTypeModifiers(
            List<FightStatModifier> result,
            ItemDefinition definition)
        {
            if (definition.BaseType == null)
            {
                return;
            }

            IReadOnlyList<FightStatModifier> modifiers =
                baseTypeResolver.Resolve(
                    definition.BaseType);

            AddModifiers(
                result,
                modifiers);
        }

        private static void AddDefinitionModifiers(
            List<FightStatModifier> result,
            ItemDefinition definition)
        {
            for (int index = 0;
                 index < definition.StatModifiers.Count;
                 index++)
            {
                CharacterStatModifierDefinition modifier =
                    definition.StatModifiers[index];

                if (modifier == null)
                {
                    throw new InvalidOperationException(
                        $"Item definition " +
                        $"{definition.ItemId.Value} contains " +
                        $"a null stat modifier.");
                }

                result.Add(
                    modifier.CreateModifier());
            }
        }

        private void AddRolledAffixModifiers(
            List<FightStatModifier> result,
            CharacterItemInstance item)
        {
            IReadOnlyList<FightStatModifier> modifiers =
                rolledAffixResolver.Resolve(item);

            AddModifiers(
                result,
                modifiers);
        }

        private static void AddModifiers(
            List<FightStatModifier> target,
            IReadOnlyList<FightStatModifier> source)
        {
            for (int index = 0;
                 index < source.Count;
                 index++)
            {
                target.Add(source[index]);
            }
        }
    }
}