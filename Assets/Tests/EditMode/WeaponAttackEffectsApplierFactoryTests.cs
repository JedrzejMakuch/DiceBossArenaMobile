using NUnit.Framework;

public sealed class WeaponAttackEffectsApplierFactoryTests
{
    [Test]
    public void Create_ReturnsEffectsApplier()
    {
        WeaponAttackEffectsApplierFactory factory =
            new WeaponAttackEffectsApplierFactory();

        WeaponAttackEffectsApplier result =
            factory.Create();

        Assert.That(
            result,
            Is.Not.Null);
    }

    [Test]
    public void Create_EachCallReturnsNewInstance()
    {
        WeaponAttackEffectsApplierFactory factory =
            new WeaponAttackEffectsApplierFactory();

        WeaponAttackEffectsApplier first =
            factory.Create();

        WeaponAttackEffectsApplier second =
            factory.Create();

        Assert.That(
            first,
            Is.Not.SameAs(second));
    }
}