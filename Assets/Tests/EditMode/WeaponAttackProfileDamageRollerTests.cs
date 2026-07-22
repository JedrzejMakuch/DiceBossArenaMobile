using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackProfileDamageRollerTests
{
    [Test]
    public void Constructor_NullDamageRollerThrows()
    {
        Assert.Throws<ArgumentNullException>(
            () =>
                new WeaponAttackProfileDamageRoller(
                    null));
    }

    [Test]
    public void Roll_NullProfileThrows()
    {
        WeaponAttackProfileDamageRoller roller =
            CreateRoller(4);

        Assert.Throws<ArgumentNullException>(
            () => roller.Roll(null));
    }

    [Test]
    public void Roll_RollsEveryLineAndPreservesMetadata()
    {
        SequenceRandomSource randomSource =
            new SequenceRandomSource(
                6,
                3);

        WeaponAttackDamageRoller damageRoller =
            new WeaponAttackDamageRoller(
                randomSource);

        WeaponAttackProfileDamageRoller roller =
            new WeaponAttackProfileDamageRoller(
                damageRoller);

        RolledWeaponProfile profile =
            new RolledWeaponProfile(
                new[]
                {
                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "primary_damage"),
                        WeaponAttackElement.Neutral,
                        4,
                        8),

                    new RolledWeaponAttackLine(
                        new WeaponAttackLineId(
                            "fire_damage"),
                        WeaponAttackElement.Fire,
                        2,
                        5)
                });

        var results =
            roller.Roll(profile);

        Assert.That(
            results.Count,
            Is.EqualTo(2));

        Assert.That(
            results[0].LineId,
            Is.EqualTo(
                new WeaponAttackLineId(
                    "primary_damage")));

        Assert.That(
            results[0].Element,
            Is.EqualTo(
                WeaponAttackElement.Neutral));

        Assert.That(
            results[0].Damage,
            Is.EqualTo(6));

        Assert.That(
            results[1].LineId,
            Is.EqualTo(
                new WeaponAttackLineId(
                    "fire_damage")));

        Assert.That(
            results[1].Element,
            Is.EqualTo(
                WeaponAttackElement.Fire));

        Assert.That(
            results[1].Damage,
            Is.EqualTo(3));
    }

    private static WeaponAttackProfileDamageRoller
        CreateRoller(int result)
    {
        WeaponAttackDamageRoller damageRoller =
            new WeaponAttackDamageRoller(
                new SequenceRandomSource(
                    result));

        return new WeaponAttackProfileDamageRoller(
            damageRoller);
    }

    private sealed class SequenceRandomSource :
        IWeaponAttackRandomSource
    {
        private readonly int[] results;
        private int currentIndex;

        public SequenceRandomSource(
            params int[] results)
        {
            this.results = results;
        }

        public int Next(
            int minimumInclusive,
            int maximumExclusive)
        {
            int result =
                results[currentIndex];

            currentIndex++;

            return result;
        }
    }
}