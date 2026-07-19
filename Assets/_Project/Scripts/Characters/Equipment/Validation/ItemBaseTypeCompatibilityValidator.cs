using System;

namespace DiceBossArena.Game
{
    public sealed class
        ItemBaseTypeCompatibilityValidator
    {
        public void Validate(
            ItemDefinition item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(
                    nameof(item));
            }

            if (item.BaseType == null)
            {
                return;
            }

            if (item.SlotType !=
                item.BaseType.SlotType)
            {
                throw new InvalidOperationException(
                    $"Item {item.ItemId.Value} uses slot " +
                    $"{item.SlotType}, but its base type " +
                    $"{item.BaseType.BaseTypeId.Value} uses " +
                    $"{item.BaseType.SlotType}.");
            }
        }
    }
}