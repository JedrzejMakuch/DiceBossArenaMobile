using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackResistanceStatResolverTests
{
    private WeaponAttackResistanceStatResolver resolver;

    [SetUp]
    public void SetUp()
    {
        resolver =
            new WeaponAttackResistanceStatResolver();
    }

    [TestCase(
        WeaponAttackElement.Fire,
        FightStatType.FireResistance)]
    [TestCase(
        WeaponAttackElement.Water,
        FightStatType.WaterResistance)]
    [TestCase(
        WeaponAttackElement.Earth,
        FightStatType.EarthResistance)]
    [TestCase(
        WeaponAttackElement.Air,
        FightStatType.AirResistance)]
    public void TryResolve_ElementalDamage_ReturnsResistanceStat(
        WeaponAttackElement element,
        FightStatType expectedStatType)
    {
        bool result = resolver.TryResolve(
            element,
            out FightStatType statType);

        Assert.That(result, Is.True);
        Assert.That(statType, Is.EqualTo(expectedStatType));
    }

    [Test]
    public void TryResolve_Neutral_ReturnsFalse()
    {
        bool result = resolver.TryResolve(
            WeaponAttackElement.Neutral,
            out _);

        Assert.That(result, Is.False);
    }

    [Test]
    public void TryResolve_UnsupportedElement_ReturnsFalse()
    {
        bool result = resolver.TryResolve(
            (WeaponAttackElement)999,
            out _);

        Assert.That(result, Is.False);
    }
}