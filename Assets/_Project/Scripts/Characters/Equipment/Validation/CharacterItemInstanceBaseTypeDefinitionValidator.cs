using System;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceBaseTypeDefinitionValidator
    {
        private readonly
            IEquipmentBaseTypeDefinitionResolver
            definitionResolver;

        public CharacterItemInstanceBaseTypeDefinitionValidator(
            IEquipmentBaseTypeDefinitionResolver
                newDefinitionResolver)
        {
            definitionResolver =
                newDefinitionResolver ??
                throw new ArgumentNullException(
                    nameof(newDefinitionResolver));
        }

        public EquipmentBaseTypeDefinition ValidateAndResolve(
            CharacterItemInstance item)
        {
            if (!item.IsValid)
            {
                throw new ArgumentException(
                    "Character item instance must be valid.",
                    nameof(item));
            }

            if (!item.BaseTypeId.IsValid)
            {
                throw new InvalidOperationException(
                    $"Item instance '{item.InstanceId}' " +
                    $"does not contain a valid equipment " +
                    $"base type id.");
            }

            if (!definitionResolver.TryResolve(
                    item.BaseTypeId,
                    out EquipmentBaseTypeDefinition definition))
            {
                throw new InvalidOperationException(
                    $"Unknown equipment base type id: " +
                    $"'{item.BaseTypeId}'.");
            }

            return definition;
        }
    }
}