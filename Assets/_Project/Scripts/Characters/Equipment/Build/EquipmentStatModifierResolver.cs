using System;
using System.Collections.Generic;

namespace DiceBossArena.Game
{
    public sealed class EquipmentStatModifierResolver
    {
        private readonly ItemDefinitionCatalog itemCatalog;

        public EquipmentStatModifierResolver(
            ItemDefinitionCatalog itemCatalog)
        {
            this.itemCatalog =
                itemCatalog ??
                throw new ArgumentNullException(
                    nameof(itemCatalog));
        }

        public IReadOnlyList<FightStatModifier> Resolve(
            EquipmentLoadoutSnapshot loadout)
        {
            List<FightStatModifier> result =
                new();

            if (loadout == null)
            {
                return result;
            }

            for (int i = 0;
                 i < loadout.Items.Count;
                 i++)
            {
                EquippedItemSnapshot equippedItem =
                    loadout.Items[i];

                if (!itemCatalog.TryResolve(
                        equippedItem.ItemId,
                        out ItemDefinition definition))
                {
                    throw new InvalidOperationException(
                        $"Could not resolve equipped item: " +
                        $"{equippedItem.ItemId.Value}.");
                }

                if (definition.BaseType != null)
                {
                    EquipmentBaseTypeStatModifierResolver
                        baseTypeResolver =
                            new EquipmentBaseTypeStatModifierResolver();

                    IReadOnlyList<FightStatModifier>
                        baseTypeModifiers =
                            baseTypeResolver.Resolve(
                                definition.BaseType);

                    for (int modifierIndex = 0;
                         modifierIndex <
                         baseTypeModifiers.Count;
                         modifierIndex++)
                    {
                        result.Add(
                            baseTypeModifiers[modifierIndex]);
                    }
                }

                for (int modifierIndex = 0;
                     modifierIndex <
                     definition.StatModifiers.Count;
                     modifierIndex++)
                {
                    CharacterStatModifierDefinition
                        modifierDefinition =
                            definition.StatModifiers[
                                modifierIndex];

                    if (modifierDefinition == null)
                    {
                        throw new InvalidOperationException(
                            $"Item " +
                            $"{equippedItem.ItemId.Value} " +
                            $"contains a null stat modifier.");
                    }

                    result.Add(
                        modifierDefinition.CreateModifier());
                }
            }

            return result;
        }
    }
}