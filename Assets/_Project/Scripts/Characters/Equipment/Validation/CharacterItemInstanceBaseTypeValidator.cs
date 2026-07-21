using System;

namespace DiceBossArena.Game
{
    public sealed class
        CharacterItemInstanceBaseTypeValidator
    {
        public void Validate(
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
        }
    }
}