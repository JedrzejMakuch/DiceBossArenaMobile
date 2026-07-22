using NUnit.Framework;

public sealed class WeaponAttackArmorCalculatorTests
{
    private WeaponAttackArmorCalculator calculator;

    [SetUp]
    public void SetUp()
    {
        calculator =
            new WeaponAttackArmorCalculator();
    }

    [TestCase(100, 0, 100)]
    [TestCase(100, 25, 75)]
    [TestCase(100, 100, 0)]
    [TestCase(100, 150, 0)]
    [TestCase(10, 3, 7)]
    public void Calculate_ValidValues_ReturnsReducedDamage(
        int damage,
        int armor,
        int expectedDamage)
    {
        int result = calculator.Calculate(
            damage,
            armor);

        Assert.That(result, Is.EqualTo(expectedDamage));
    }

    [Test]
    public void Calculate_NegativeArmor_IsTreatedAsZero()
    {
        int result = calculator.Calculate(
            100,
            -25);

        Assert.That(result, Is.EqualTo(100));
    }

    [TestCase(0)]
    [TestCase(-10)]
    public void Calculate_NonPositiveDamage_ReturnsZero(
        int damage)
    {
        int result = calculator.Calculate(
            damage,
            25);

        Assert.That(result, Is.Zero);
    }
}