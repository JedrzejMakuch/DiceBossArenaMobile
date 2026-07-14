using NUnit.Framework;

public class FightUnitRuntimeSnapshotTests
{
    [Test]
    public void Fresh_DoesNotOverrideCurrentHealth()
    {
        FightUnitRuntimeSnapshot snapshot =
            FightUnitRuntimeSnapshot.Fresh;

        Assert.That(
            snapshot.HasCurrentHealth,
            Is.False);

        Assert.That(
            snapshot.CurrentHealth,
            Is.EqualTo(0));
    }

    [Test]
    public void Constructor_PreservesCurrentHealth()
    {
        FightUnitRuntimeSnapshot snapshot =
            new FightUnitRuntimeSnapshot(7);

        Assert.That(
            snapshot.HasCurrentHealth,
            Is.True);

        Assert.That(
            snapshot.CurrentHealth,
            Is.EqualTo(7));
    }

    [Test]
    public void Constructor_AllowsDeadState()
    {
        FightUnitRuntimeSnapshot snapshot =
            new FightUnitRuntimeSnapshot(0);

        Assert.That(
            snapshot.HasCurrentHealth,
            Is.True);

        Assert.That(
            snapshot.CurrentHealth,
            Is.EqualTo(0));
    }

    [Test]
    public void Constructor_ClampsNegativeHealthToZero()
    {
        FightUnitRuntimeSnapshot snapshot =
            new FightUnitRuntimeSnapshot(-10);

        Assert.That(
            snapshot.CurrentHealth,
            Is.EqualTo(0));
    }

    [Test]
    public void SnapshotsWithSameState_AreEqual()
    {
        FightUnitRuntimeSnapshot first =
            new FightUnitRuntimeSnapshot(7);

        FightUnitRuntimeSnapshot second =
            new FightUnitRuntimeSnapshot(7);

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void FreshAndDeadState_AreNotEqual()
    {
        FightUnitRuntimeSnapshot fresh =
            FightUnitRuntimeSnapshot.Fresh;

        FightUnitRuntimeSnapshot dead =
            new FightUnitRuntimeSnapshot(0);

        Assert.That(
            fresh,
            Is.Not.EqualTo(dead));
    }
}