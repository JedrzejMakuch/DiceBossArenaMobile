using DiceBossArena.Game;
using System;

namespace DiceBossArena.Assets._Project.Scripts.Characters.Equipment.Definitions
{
    public sealed class CharacterItemInstanceValidator
    {
        private readonly IItemDefinitionResolver
            definitionResolver;

        public CharacterItemInstanceValidator(
            IItemDefinitionResolver definitionResolver)
        {
            this.definitionResolver =
                definitionResolver ??
                throw new ArgumentNullException(
                    nameof(definitionResolver));
        }

        public ItemDefinition ValidateAndResolve(
            CharacterItemInstance instance)
        {
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

            return definition;
        }
    }
}