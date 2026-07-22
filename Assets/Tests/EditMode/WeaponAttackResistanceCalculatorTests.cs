using NUnit.Framework;

public sealed class WeaponAttackResistanceCalculatorTests
{
    private WeaponAttackResistanceCalculator calculator;

    [SetUp]
    public void SetUp()
    {
        calculator =
            new WeaponAttackResistanceCalculator();
    }

    [TestCase(100, 0, 100)]
    [TestCase(100, 25, 75)]
    [TestCase(100, 50, 50)]
    [TestCase(100, 100, 0)]
    [TestCase(10, 33, 6)]
    public void Calculate_ValidValues_ReturnsReducedDamage(
        int damage,
        int resistancePercent,
        int expectedDamage)
    {
        int result = calculator.Calculate(
            damage,
            resistancePercent);

        Assert.That(result, Is.EqualTo(expectedDamage));
    }

    [Test]
    public void Calculate_NegativeResistance_ClampsToZero()
    {
        int result = calculator.Calculate(
            100,
            -25);

        Assert.That(result, Is.EqualTo(100));
    }

    [Test]
    public void Calculate_ResistanceAboveMaximum_ClampsToOneHundred()
    {
        int result = calculator.Calculate(
            100,
            150);

        Assert.That(result, Is.Zero);
    }

    [TestCase(0)]
    [TestCase(-10)]
    public void Calculate_NonPositiveDamage_ReturnsZero(
        int damage)
    {
        int result = calculator.Calculate(
            damage,
            50);

        Assert.That(result, Is.Zero);
    }
}