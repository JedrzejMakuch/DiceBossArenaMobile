using System;

namespace DiceBossArena.Game
{
    public sealed class CharacterItemInstanceGenerator
    {
        private readonly ICharacterItemInstanceIdGenerator
            instanceIdGenerator;

        private readonly EquipmentItemRarityRoller
            rarityRoller;

        private readonly EquipmentAffixCollectionByRarityGenerator
            affixGenerator;

        public CharacterItemInstanceGenerator(
            ICharacterItemInstanceIdGenerator
                newInstanceIdGenerator,
            EquipmentItemRarityRoller
                newRarityRoller,
            EquipmentAffixCollectionByRarityGenerator
                newAffixGenerator)
        {
            instanceIdGenerator =
                newInstanceIdGenerator ??
                throw new ArgumentNullException(
                    nameof(newInstanceIdGenerator));

            rarityRoller =
                newRarityRoller ??
                throw new ArgumentNullException(
                    nameof(newRarityRoller));

            affixGenerator =
                newAffixGenerator ??
                throw new ArgumentNullException(
                    nameof(newAffixGenerator));
        }

        public CharacterItemInstance Generate(
            CharacterItemInstanceGenerationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(
                    nameof(request));
            }

            EquipmentItemRarity rarity =
                rarityRoller.Roll();

            var affixes =
                affixGenerator.Generate(
                    request.AffixPool,
                    request.Level,
                    rarity);

            return new CharacterItemInstance(
                instanceIdGenerator.Generate(),
                request.ItemDefinition.ItemId,
                request.ItemDefinition.BaseType.BaseTypeId,
                request.Level,
                request.UpgradeLevel,
                request.Quantity,
                rarity,
                affixes);
        }
    }
}