using System;

namespace DiceBossArena.Game
{
    public sealed class EquipmentAffixGenerator
    {
        private readonly EquipmentAffixTierResolver
            tierResolver =
                new EquipmentAffixTierResolver();

        private readonly EquipmentAffixTierValueRoller
            valueRoller;

        public EquipmentAffixGenerator(
            IEquipmentAffixRandomSource randomSource)
        {
            valueRoller =
                new EquipmentAffixTierValueRoller(
                    randomSource ??
                    throw new ArgumentNullException(
                        nameof(randomSource)));
        }

        public RolledEquipmentAffix Generate(
            EquipmentAffixDefinition definition,
            int itemLevel)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(
                    nameof(definition));
            }

            if (itemLevel < 1)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(itemLevel),
                    itemLevel,
                    "Item level must be at least 1.");
            }

            EquipmentAffixTierDefinition tier =
                tierResolver.Resolve(
                    definition,
                    itemLevel);

            if (tier == null)
            {
                return null;
            }

            int value =
                valueRoller.Roll(tier);

            return new RolledEquipmentAffix(
                definition.Id,
                definition.StatType,
                definition.ModifierType,
                value);
        }
    }
}