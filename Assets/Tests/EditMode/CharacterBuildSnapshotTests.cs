using System.Collections.Generic;
using DiceBossArena.Game;
using NUnit.Framework;

public class CharacterBuildSnapshotTests
{
    [Test]
    public void Snapshot_WithSkillIsNotEmpty()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId(
                    string.Empty),
                new CharacterSpecializationId(
                    string.Empty),
                new[]
                {
                new CharacterBuildSkill(
                    "shield_bash",
                    1)
                },
                null);

        Assert.That(
            snapshot.IsEmpty,
            Is.False);
    }

    [Test]
    public void Snapshot_WithClassIsNotEmpty()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    string.Empty),
                null,
                null);

        Assert.That(
            snapshot.IsEmpty,
            Is.False);
    }

    [Test]
    public void Empty_IsEmptyReturnsTrue()
    {
        CharacterBuildSnapshot snapshot =
            CharacterBuildSnapshot.Empty;

        Assert.That(
            snapshot.IsEmpty,
            Is.True);
    }

    [Test]
    public void Constructor_CopiesClassAndSpecializationIds()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId("guardian"),
                null,
                null);

        Assert.That(
            snapshot.ClassId.Value,
            Is.EqualTo("warrior"));

        Assert.That(
            snapshot.SpecializationId.Value,
            Is.EqualTo("guardian"));
    }

    [Test]
    public void Constructor_CopiesSkillCollection()
    {
        List<CharacterBuildSkill> sourceSkills =
            new()
            {
                new CharacterBuildSkill(
                    "basic_attack",
                    1)
            };

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId("guardian"),
                sourceSkills,
                null);

        sourceSkills.Add(
            new CharacterBuildSkill(
                "shield_bash",
                2));

        Assert.That(
            snapshot.Skills.Count,
            Is.EqualTo(1));

        Assert.That(
            snapshot.Skills[0].SkillId,
            Is.EqualTo("basic_attack"));
    }

    [Test]
    public void Constructor_CopiesStatModifierCollection()
    {
        List<FightStatModifier> sourceModifiers =
            new()
            {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)
            };

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId("guardian"),
                null,
                sourceModifiers);

        sourceModifiers.Add(
            new FightStatModifier(
                FightStatType.AttackPower,
                FightStatModifierType.Flat,
                5));

        Assert.That(
            snapshot.StatModifiers.Count,
            Is.EqualTo(1));

        Assert.That(
            snapshot.StatModifiers[0].StatType,
            Is.EqualTo(
                FightStatType.MaxHealth));
    }

    [Test]
    public void Constructor_NullCollectionsCreateEmptySnapshot()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId("guardian"),
                null,
                null);

        Assert.That(
            snapshot.Skills,
            Is.Empty);

        Assert.That(
            snapshot.StatModifiers,
            Is.Empty);
    }

    [Test]
    public void Collections_CannotBeModifiedThroughSnapshot()
    {
        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId("guardian"),
                new[]
                {
                new CharacterBuildSkill(
                    "basic_attack",
                    1)
                },
                new[]
                {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)
                });

        Assert.That(
            snapshot.Skills,
            Is.Not.InstanceOf<
                List<CharacterBuildSkill>>());

        Assert.That(
            snapshot.StatModifiers,
            Is.Not.InstanceOf<
                List<FightStatModifier>>());
    }

    [Test]
    public void Constructor_DuplicateSkillIdThrowsException()
    {
        CharacterBuildSkill[] skills =
        {
        new CharacterBuildSkill(
            "basic_attack",
            1),
        new CharacterBuildSkill(
            "basic_attack",
            2)
    };

        Assert.That(
            () =>
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    skills,
                    null),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_InvalidSkillThrowsException()
    {
        CharacterBuildSkill[] skills =
        {
        new CharacterBuildSkill(
            "   ",
            1)
    };

        Assert.That(
            () =>
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    skills,
                    null),
            Throws.ArgumentException);
    }

    [Test]
    public void SnapshotsWithSameContent_AreEqual()
    {
        CharacterBuildSnapshot first =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                new CharacterBuildSkill(
                    "basic_attack",
                    1),
                new CharacterBuildSkill(
                    "shield_bash",
                    2)
                },
                new[]
                {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)
                });

        CharacterBuildSnapshot second =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                new CharacterBuildSkill(
                    "basic_attack",
                    1),
                new CharacterBuildSkill(
                    "shield_bash",
                    2)
                },
                new[]
                {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10)
                });

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void SnapshotsWithDifferentSkillOrder_AreNotEqual()
    {
        CharacterBuildSnapshot first =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                new CharacterBuildSkill(
                    "basic_attack",
                    1),
                new CharacterBuildSkill(
                    "shield_bash",
                    2)
                },
                null);

        CharacterBuildSnapshot second =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                new CharacterBuildSkill(
                    "shield_bash",
                    2),
                new CharacterBuildSkill(
                    "basic_attack",
                    1)
                },
                null);

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    [Test]
    public void Constructor_CopiesEquipmentAndPassives()
    {
        EquipmentLoadoutSnapshot loadout =
            new EquipmentLoadoutSnapshot(
                new[]
                {
                new EquippedItemSnapshot(
                    EquipmentSlotType.MainHand,
                    new CharacterItemId(
                        "iron_sword"))
                });

        List<CharacterPassiveId> passiveIds =
            new()
            {
            new CharacterPassiveId(
                "heavy_armor_mastery")
            };

        CharacterBuildSnapshot snapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                loadout,
                passiveIds);

        passiveIds.Add(
            new CharacterPassiveId(
                "shield_mastery"));

        Assert.That(
            snapshot.EquipmentLoadout.Items.Count,
            Is.EqualTo(1));

        Assert.That(
            snapshot.EquipmentLoadout.Items[0]
                .ItemId.Value,
            Is.EqualTo("iron_sword"));

        Assert.That(
            snapshot.PassiveIds.Count,
            Is.EqualTo(1));

        Assert.That(
            snapshot.PassiveIds[0].Value,
            Is.EqualTo(
                "heavy_armor_mastery"));
    }

    [Test]
    public void Constructor_DuplicatePassiveIdThrowsException()
    {
        CharacterPassiveId[] passiveIds =
        {
        new CharacterPassiveId(
            "shield_mastery"),
        new CharacterPassiveId(
            "shield_mastery")
    };

        Assert.That(
            () =>
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    null,
                    null,
                    null,
                    passiveIds),
            Throws.ArgumentException);
    }

    [Test]
    public void Constructor_InvalidPassiveIdThrowsException()
    {
        CharacterPassiveId[] passiveIds =
        {
        new CharacterPassiveId("   ")
    };

        Assert.That(
            () =>
                new CharacterBuildSnapshot(
                    new CharacterClassId("warrior"),
                    new CharacterSpecializationId(
                        "guardian"),
                    null,
                    null,
                    null,
                    passiveIds),
            Throws.ArgumentException);
    }

    [Test]
    public void SnapshotsWithDifferentEquipment_AreNotEqual()
    {
        CharacterBuildSnapshot first =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                new EquipmentLoadoutSnapshot(
                    new[]
                    {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.MainHand,
                        new CharacterItemId(
                            "iron_sword"))
                    }));

        CharacterBuildSnapshot second =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                new EquipmentLoadoutSnapshot(
                    new[]
                    {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.MainHand,
                        new CharacterItemId(
                            "wooden_staff"))
                    }));

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    [Test]
    public void SnapshotsWithDifferentPassives_AreNotEqual()
    {
        CharacterBuildSnapshot first =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                null,
                new[]
                {
                new CharacterPassiveId(
                    "shield_mastery")
                });

        CharacterBuildSnapshot second =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                null,
                null,
                null,
                new[]
                {
                new CharacterPassiveId(
                    "heavy_armor_mastery")
                });

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    [Test]
    public void Empty_CreatesValidEmptyCollections()
    {
        CharacterBuildSnapshot snapshot =
            CharacterBuildSnapshot.Empty;

        Assert.That(
            snapshot.ClassId.IsValid,
            Is.False);

        Assert.That(
            snapshot.SpecializationId.IsValid,
            Is.False);

        Assert.That(
            snapshot.Skills,
            Is.Empty);

        Assert.That(
            snapshot.StatModifiers,
            Is.Empty);

        Assert.That(
            snapshot.EquipmentLoadout.Items,
            Is.Empty);

        Assert.That(
            snapshot.PassiveIds,
            Is.Empty);
    }

    [Test]
    public void Copy_RecreatesEqualIndependentSnapshot()
    {
        CharacterBuildSnapshot original =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
                new CharacterBuildSkill(
                    "basic_attack",
                    1),
                new CharacterBuildSkill(
                    "shield_bash",
                    2)
                },
                new[]
                {
                new FightStatModifier(
                    FightStatType.MaxHealth,
                    FightStatModifierType.Flat,
                    10),
                new FightStatModifier(
                    FightStatType.AttackPower,
                    FightStatModifierType.Percent,
                    20)
                },
                new EquipmentLoadoutSnapshot(
                    new[]
                    {
                    new EquippedItemSnapshot(
                        EquipmentSlotType.MainHand,
                        new CharacterItemId(
                            "iron_sword")),
                    new EquippedItemSnapshot(
                        EquipmentSlotType.Armor,
                        new CharacterItemId(
                            "guardian_armor"))
                    }),
                new[]
                {
                new CharacterPassiveId(
                    "shield_mastery"),
                new CharacterPassiveId(
                    "heavy_armor_mastery")
                });

        CharacterBuildSnapshot copy =
            original.Copy();

        Assert.That(
            copy,
            Is.EqualTo(original));

        Assert.That(
            copy.GetHashCode(),
            Is.EqualTo(
                original.GetHashCode()));

        Assert.That(
            ReferenceEquals(original, copy),
            Is.False);

        Assert.That(
            ReferenceEquals(
                original.Skills,
                copy.Skills),
            Is.False);

        Assert.That(
            ReferenceEquals(
                original.StatModifiers,
                copy.StatModifiers),
            Is.False);

        Assert.That(
            ReferenceEquals(
                original.PassiveIds,
                copy.PassiveIds),
            Is.False);
    }

    [Test]
    public void Copy_PreservesCollectionOrder()
    {
        CharacterBuildSnapshot original =
            new CharacterBuildSnapshot(
                new CharacterClassId("mage"),
                new CharacterSpecializationId(
                    "pyromancer"),
                new[]
                {
                new CharacterBuildSkill(
                    "fireball",
                    3),
                new CharacterBuildSkill(
                    "flame_wall",
                    2)
                },
                null,
                null,
                new[]
                {
                new CharacterPassiveId(
                    "burning_focus"),
                new CharacterPassiveId(
                    "mana_flow")
                });

        CharacterBuildSnapshot copy =
            original.Copy();

        Assert.That(
            copy.Skills[0].SkillId,
            Is.EqualTo("fireball"));

        Assert.That(
            copy.Skills[1].SkillId,
            Is.EqualTo("flame_wall"));

        Assert.That(
            copy.PassiveIds[0].Value,
            Is.EqualTo("burning_focus"));

        Assert.That(
            copy.PassiveIds[1].Value,
            Is.EqualTo("mana_flow"));
    }
}