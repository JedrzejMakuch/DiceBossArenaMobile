using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    CharacterItemInstanceBaseTypeValidatorTests
{
    [Test]
    public void Validate_InvalidItemThrows()
    {
        CharacterItemInstanceBaseTypeValidator validator =
            new CharacterItemInstanceBaseTypeValidator();

        Assert.That(
            () => validator.Validate(default),
            Throws.TypeOf<ArgumentException>());
    }

    [Test]
    public void Validate_ItemWithoutBaseTypeThrows()
    {
        CharacterItemInstance item =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                1,
                0,
                1);

        CharacterItemInstanceBaseTypeValidator validator =
            new CharacterItemInstanceBaseTypeValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.TypeOf<InvalidOperationException>());
    }

    [Test]
    public void Validate_ItemWithBaseTypeDoesNotThrow()
    {
        CharacterItemInstance item =
            new CharacterItemInstance(
                new CharacterItemInstanceId(
                    "instance_001"),
                new CharacterItemId(
                    "iron_sword"),
                new EquipmentBaseTypeId(
                    "one_handed_sword"),
                1,
                0,
                1,
                EquipmentItemRarity.Common,
                Array.Empty<RolledEquipmentAffix>());

        CharacterItemInstanceBaseTypeValidator validator =
            new CharacterItemInstanceBaseTypeValidator();

        Assert.That(
            () => validator.Validate(item),
            Throws.Nothing);
    }
}