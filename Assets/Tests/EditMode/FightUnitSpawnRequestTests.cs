using DiceBossArena.Game;
using NUnit.Framework;
using UnityEngine;

public class FightUnitSpawnRequestTests
{
    [Test]
    public void Constructor_NullSnapshotsUseEmptyDefaults()
    {
        FightUnitSpawnRequest request =
            new FightUnitSpawnRequest(
                null,
                null,
                null,
                null);

        Assert.That(
            request.RuntimeSnapshot,
            Is.Not.Null);

        Assert.That(
            request.RuntimeSnapshot.HasCurrentHealth,
            Is.False);

        Assert.That(
            request.BuildSnapshot,
            Is.Not.Null);

        Assert.That(
            request.BuildSnapshot,
            Is.EqualTo(
                CharacterBuildSnapshot.Empty));
    }

    [Test]
    public void Constructor_PreservesRuntimeSnapshot()
    {
        FightUnitRuntimeSnapshot runtimeSnapshot =
            new FightUnitRuntimeSnapshot(6);

        FightUnitSpawnRequest request =
            new FightUnitSpawnRequest(
                null,
                null,
                null,
                null,
                runtimeSnapshot:
                    runtimeSnapshot);

        Assert.That(
            request.RuntimeSnapshot,
            Is.SameAs(runtimeSnapshot));

        Assert.That(
            request.RuntimeSnapshot.CurrentHealth,
            Is.EqualTo(6));
    }

    [Test]
    public void Constructor_CopiesBuildSnapshot()
    {
        CharacterBuildSnapshot buildSnapshot =
            new CharacterBuildSnapshot(
                new CharacterClassId("warrior"),
                new CharacterSpecializationId(
                    "guardian"),
                new[]
                {
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

        FightUnitSpawnRequest request =
            new FightUnitSpawnRequest(
                null,
                null,
                null,
                null,
                buildSnapshot:
                    buildSnapshot);

        Assert.That(
            request.BuildSnapshot,
            Is.EqualTo(buildSnapshot));

        Assert.That(
            ReferenceEquals(
                request.BuildSnapshot,
                buildSnapshot),
            Is.False);

        Assert.That(
            ReferenceEquals(
                request.BuildSnapshot.Skills,
                buildSnapshot.Skills),
            Is.False);
    }

    [Test]
    public void ExistingConstructorUsageGetsDefaultSnapshots()
    {
        GameObject parentObject =
            new GameObject("Parent");

        FightUnitSpawnRequest request =
            new FightUnitSpawnRequest(
                prefab: null,
                definition: null,
                ownership: null,
                tile: null,
                parent:
                    parentObject.transform,
                objectName:
                    "Test Unit");

        Assert.That(
            request.Parent,
            Is.SameAs(
                parentObject.transform));

        Assert.That(
            request.ObjectName,
            Is.EqualTo("Test Unit"));

        Assert.That(
            request.RuntimeSnapshot,
            Is.Not.Null);

        Assert.That(
            request.BuildSnapshot,
            Is.Not.Null);

        Object.DestroyImmediate(parentObject);
    }
}