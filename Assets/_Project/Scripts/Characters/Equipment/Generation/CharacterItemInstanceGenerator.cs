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

        private readonly WeaponProfileRoller
    weaponProfileRoller;

        public CharacterItemInstanceGenerator(
            ICharacterItemInstanceIdGenerator
                newInstanceIdGenerator,
            EquipmentItemRarityRoller
                newRarityRoller,
            EquipmentAffixCollectionByRarityGenerator
                newAffixGenerator,
            WeaponProfileRoller
    newWeaponProfileRoller)
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

            weaponProfileRoller =
                newWeaponProfileRoller ??
                throw new ArgumentNullException(
                    nameof(newWeaponProfileRoller));
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

            RolledWeaponProfile weaponProfile =
                request.ItemDefinition.BaseType
                    .WeaponProfileGeneration == null
                    ? null
                    : weaponProfileRoller.Roll(
                        request.ItemDefinition.BaseType
                            .WeaponProfileGeneration);

            return new CharacterItemInstance(
                instanceIdGenerator.Generate(),
                request.ItemDefinition.ItemId,
                request.ItemDefinition.BaseType.BaseTypeId,
                request.Level,
                request.UpgradeLevel,
                request.Quantity,
                rarity,
                affixes,
                weaponProfile);
        }
    }
}