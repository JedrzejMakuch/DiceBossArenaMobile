using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class CharacterEquipmentStatModifierResolver
    {
        private readonly CharacterItemBuildStatModifierResolver
            itemResolver;

        public CharacterEquipmentStatModifierResolver(
            ItemDefinitionCatalog itemCatalog)
        {
            if (itemCatalog == null)
            {
                throw new ArgumentNullException(
                    nameof(itemCatalog));
            }

            itemResolver =
                new CharacterItemBuildStatModifierResolver(
                    itemCatalog);
        }

        public IReadOnlyList<FightStatModifier> Resolve(
            CharacterInventory inventory,
            CharacterEquipmentLoadout loadout)
        {
            if (inventory == null)
            {
                throw new ArgumentNullException(
                    nameof(inventory));
            }

            if (loadout == null)
            {
                throw new ArgumentNullException(
                    nameof(loadout));
            }

            List<FightStatModifier> result =
                new List<FightStatModifier>();

            for (int itemIndex = 0;
                 itemIndex < loadout.Items.Count;
                 itemIndex++)
            {
                EquippedItemInstance equippedItem =
                    loadout.Items[itemIndex];

                if (!inventory.TryGet(
                        equippedItem.InstanceId,
                        out CharacterItemInstance item))
                {
                    throw new InvalidOperationException(
                        $"Could not resolve equipped item " +
                        $"instance: " +
                        $"{equippedItem.InstanceId.Value}.");
                }

                IReadOnlyList<FightStatModifier>
                    itemModifiers =
                        itemResolver.Resolve(item);

                for (int modifierIndex = 0;
                     modifierIndex <
                     itemModifiers.Count;
                     modifierIndex++)
                {
                    result.Add(
                        itemModifiers[modifierIndex]);
                }
            }

            return result;
        }
    }
}