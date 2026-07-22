using System;
using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackDamageRollerTests
{
    [Test]
    public void Constructor_NullRandomSourceThrows()
    {
        Assert.Throws<ArgumentNullException>(
            () => new WeaponAttackDamageRoller(null));
    }

    [Test]
    public void Roll_NullAttackLineThrows()
    {
        WeaponAttackDamageRoller roller =
            new WeaponAttackDamageRoller(
                new TestRandomSource(4));

        Assert.Throws<ArgumentNullException>(
            () => roller.Roll(null));
    }

    [Test]
    public void Roll_UsesInclusiveDamageRange()
    {
        TestRandomSource randomSource =
            new TestRandomSource(7);

        WeaponAttackDamageRoller roller =
            new WeaponAttackDamageRoller(
                randomSource);

        RolledWeaponAttackLine attackLine =
            new RolledWeaponAttackLine(
                new WeaponAttackLineId(
                    "primary_damage"),
                WeaponAttackElement.Neutral,
                3,
                7);

        int result =
            roller.Roll(attackLine);

        Assert.That(
            result,
            Is.EqualTo(7));

        Assert.That(
            randomSource.MinimumInclusive,
            Is.EqualTo(3));

        Assert.That(
            randomSource.MaximumExclusive,
            Is.EqualTo(8));
    }

    private sealed class TestRandomSource :
        IWeaponAttackRandomSource
    {
        private readonly int result;

        public int MinimumInclusive { get; private set; }

        public int MaximumExclusive { get; private set; }

        public TestRandomSource(int result)
        {
            this.result = result;
        }

        public int Next(
            int minimumInclusive,
            int maximumExclusive)
        {
            MinimumInclusive =
                minimumInclusive;

            MaximumExclusive =
                maximumExclusive;

            return result;
        }
    }
}