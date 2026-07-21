using System;

namespace DiceBossArena.Game
{
    public sealed class CharacterItemInstanceValidator
    {
        private readonly IItemDefinitionResolver
            definitionResolver;

        private readonly CharacterItemInstanceAffixValidator
            affixValidator;

        private readonly
            CharacterItemInstanceBaseTypeDefinitionValidator
            baseTypeValidator;

        public CharacterItemInstanceValidator(
            IItemDefinitionResolver definitionResolver)
        {
            this.definitionResolver =
                definitionResolver ??
                throw new ArgumentNullException(
                    nameof(definitionResolver));
        }

        public CharacterItemInstanceValidator(
            IItemDefinitionResolver definitionResolver,
            EquipmentAffixDefinitionCatalog affixCatalog)
        {
            this.definitionResolver =
                definitionResolver ??
                throw new ArgumentNullException(
                    nameof(definitionResolver));

            if (affixCatalog == null)
            {
                throw new ArgumentNullException(
                    nameof(affixCatalog));
            }

            affixValidator =
                new CharacterItemInstanceAffixValidator(
                    affixCatalog);
        }

        public CharacterItemInstanceValidator(
            IItemDefinitionResolver definitionResolver,
            EquipmentAffixDefinitionCatalog affixCatalog,
            IEquipmentBaseTypeDefinitionResolver
                baseTypeDefinitionResolver)
        {
            this.definitionResolver =
                definitionResolver ??
                throw new ArgumentNullException(
                    nameof(definitionResolver));

            if (affixCatalog == null)
            {
                throw new ArgumentNullException(
                    nameof(affixCatalog));
            }

            if (baseTypeDefinitionResolver == null)
            {
                throw new ArgumentNullException(
                    nameof(baseTypeDefinitionResolver));
            }

            affixValidator =
                new CharacterItemInstanceAffixValidator(
                    affixCatalog);

            baseTypeValidator =
                new CharacterItemInstanceBaseTypeDefinitionValidator(
                    baseTypeDefinitionResolver);
        }

        public ItemDefinition ValidateAndResolve(
            CharacterItemInstance instance)
        {
            if (!instance.IsValid)
            {
                throw new ArgumentException(
                    "Character item instance must be valid.",
                    nameof(instance));
            }

            if (!definitionResolver.TryResolve(
                    instance.ItemId,
                    out ItemDefinition definition))
            {
                throw new InvalidOperationException(
                    $"Unknown item definition: " +
                    $"{instance.ItemId}.");
            }

            if (instance.Quantity >
                definition.MaxStackSize)
            {
                throw new InvalidOperationException(
                    $"Item instance {instance.InstanceId} " +
                    $"has quantity {instance.Quantity}, " +
                    $"but definition {definition.ItemId} " +
                    $"allows a maximum stack of " +
                    $"{definition.MaxStackSize}.");
            }

            baseTypeValidator?.ValidateAndResolve(
                instance);

            affixValidator?.Validate(
                instance);

            return definition;
        }
    }
}