using DiceBossArena.Game;
using NUnit.Framework;

public sealed class WeaponAttackDamageLineResolverTests
{
    private WeaponAttackDamageLineResolver resolver;

    [SetUp]
    public void SetUp()
    {
        resolver =
            new WeaponAttackDamageLineResolver();
    }

    [Test]
    public void Resolve_NullDamageLineReturnsZero()
    {
        int result =
            resolver.Resolve(null);

        Assert.That(
            result,
            Is.EqualTo(0));
    }

    [Test]
    public void Resolve_PositiveDamageReturnsUnchangedDamage()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Fire,
                7);

        int result =
            resolver.Resolve(
                damageLine);

        Assert.That(
            result,
            Is.EqualTo(7));
    }

    [Test]
    public void Resolve_ZeroDamageReturnsZero()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Neutral,
                0);

        int result =
            resolver.Resolve(
                damageLine);

        Assert.That(
            result,
            Is.EqualTo(0));
    }

    [Test]
    public void Resolve_NegativeDamageReturnsZero()
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                WeaponAttackElement.Neutral,
                -4);

        int result =
            resolver.Resolve(
                damageLine);

        Assert.That(
            result,
            Is.EqualTo(0));
    }

    [TestCase(WeaponAttackElement.Neutral)]
    [TestCase(WeaponAttackElement.Fire)]
    public void Resolve_DoesNotChangeDamageBasedOnElementYet(
        WeaponAttackElement element)
    {
        WeaponAttackDamageLineResult damageLine =
            CreateDamageLine(
                element,
                5);

        int result =
            resolver.Resolve(
                damageLine);

        Assert.That(
            result,
            Is.EqualTo(5));
    }

    private static WeaponAttackDamageLineResult
        CreateDamageLine(
            WeaponAttackElement element,
            int damage)
    {
        return new WeaponAttackDamageLineResult(
            new WeaponAttackLineId(
                "test_damage"),
            element,
            damage);
    }
}