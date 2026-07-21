using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public static class
        EquipmentLoadoutSnapshotFactory
    {
        public static EquipmentLoadoutSnapshot Create(
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

            List<EquippedItemSnapshot> items =
                new();

            for (int i = 0;
                 i < loadout.Items.Count;
                 i++)
            {
                EquippedItemInstance equippedItem =
                    loadout.Items[i];

                if (!inventory.TryGet(
                        equippedItem.InstanceId,
                        out CharacterItemInstance item))
                {
                    throw new InvalidOperationException(
                        $"Equipped item instance " +
                        $"{equippedItem.InstanceId} " +
                        "was not found in inventory.");
                }

                items.Add(
                    new EquippedItemSnapshot(
                        equippedItem.SlotType,
                        item.ItemId,
                        item.WeaponProfile));
            }

            return new EquipmentLoadoutSnapshot(
                items);
        }
    }
}