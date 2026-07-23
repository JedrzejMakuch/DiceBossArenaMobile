using DiceBossArena.Game;
using NUnit.Framework;
using System;
using System.Collections.Generic;

public sealed class CharacterItemStatModifierResolverTests
{
    private CharacterItemStatModifierResolver resolver;

    [SetUp]
    public void SetUp()
    {
        resolver =
            new CharacterItemStatModifierResolver();
    }

    [Test]
    public void Resolve_InvalidItemThrows()
    {
        Assert.Throws<ArgumentException>(
            () => resolver.Resolve(default));
    }

    [Test]
    public void Resolve_ItemWithoutAffixesReturnsEmptyCollection()
    {
        CharacterItemInstance item =
            CreateItem(
                Array.Empty<RolledEquipmentAffix>());

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(item);

        Assert.That(
            result,
            Is.Not.Null);

        Assert.That(
            result,
            Is.Empty);
    }

    [Test]
    public void Resolve_SingleAffixCreatesMatchingModifier()
    {
        CharacterItemInstance item =
            CreateItem(
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        7)
                });

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(item);

        Assert.That(
            result.Count,
            Is.EqualTo(1));

        Assert.That(
            result[0],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Strength,
                    FightStatModifierType.Flat,
                    7)));
    }

    [Test]
    public void Resolve_MultipleAffixesPreservesOrder()
    {
        CharacterItemInstance item =
            CreateItem(
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "health_flat"),
                        FightStatType.MaxHealth,
                        FightStatModifierType.Flat,
                        15),

                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "fire_resistance_percent"),
                        FightStatType.FireResistance,
                        FightStatModifierType.Percent,
                        10),

                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "initiative_flat"),
                        FightStatType.Initiative,
                        FightStatModifierType.Flat,
                        4)
                });

        IReadOnlyList<FightStatModifier> result =
            resolver.Resolve(item);

        Assert.That(
            result.Count,
            Is.EqualTo(3));

        Assert.That(
            result[0],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    15)));

        Assert.That(
            result[1],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.FireResistance,
                    FightStatModifierType.Percent,
                    10)));

        Assert.That(
            result[2],
            Is.EqualTo(
                new FightStatModifier(
                    FightStatType.Initiative,
                    FightStatModifierType.Flat,
                    4)));
    }

    [Test]
    public void Resolve_EachCallReturnsNewCollection()
    {
        CharacterItemInstance item =
            CreateItem(
                new[]
                {
                    new RolledEquipmentAffix(
                        new EquipmentAffixId(
                            "strength_flat"),
                        FightStatType.Strength,
                        FightStatModifierType.Flat,
                        5)
                });

        IReadOnlyList<FightStatModifier> first =
            resolver.Resolve(item);

        IReadOnlyList<FightStatModifier> second =
            resolver.Resolve(item);

        Assert.That(
            first,
            Is.Not.SameAs(second));
    }

    private static CharacterItemInstance CreateItem(
        IReadOnlyList<RolledEquipmentAffix> affixes)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                "test_instance"),
            new CharacterItemId(
                "test_item"),
            new EquipmentBaseTypeId(
                "test_base_type"),
            level: 1,
            upgradeLevel: 0,
            quantity: 1,
            rarity: EquipmentItemRarity.Common,
            newAffixes: affixes);
    }
}