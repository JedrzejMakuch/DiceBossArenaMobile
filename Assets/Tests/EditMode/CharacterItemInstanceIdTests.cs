using DiceBossArena.Game;
using NUnit.Framework;
using System;

public class CharacterItemInstanceIdTests
{
    [Test]
    public void Constructor_TrimsValue()
    {
        CharacterItemInstanceId id =
            new CharacterItemInstanceId(
                "  item_instance_001  ");

        Assert.That(
            id.Value,
            Is.EqualTo("item_instance_001"));
    }

    [Test]
    public void EmptyValue_IsInvalid()
    {
        CharacterItemInstanceId id =
            new CharacterItemInstanceId("   ");

        Assert.That(
            id.IsValid,
            Is.False);
    }

    [Test]
    public void SameValues_AreEqual()
    {
        CharacterItemInstanceId first =
            new CharacterItemInstanceId(
                "item_instance_001");

        CharacterItemInstanceId second =
            new CharacterItemInstanceId(
                "item_instance_001");

        Assert.That(
            first,
            Is.EqualTo(second));

        Assert.That(
            first.GetHashCode(),
            Is.EqualTo(second.GetHashCode()));
    }

    [Test]
    public void DifferentValues_AreNotEqual()
    {
        CharacterItemInstanceId first =
            new CharacterItemInstanceId(
                "item_instance_001");

        CharacterItemInstanceId second =
            new CharacterItemInstanceId(
                "item_instance_002");

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }

    [Test]
    public void Comparison_IsCaseSensitive()
    {
        CharacterItemInstanceId first =
            new CharacterItemInstanceId(
                "item_instance_001");

        CharacterItemInstanceId second =
            new CharacterItemInstanceId(
                "ITEM_INSTANCE_001");

        Assert.That(
            first,
            Is.Not.EqualTo(second));
    }
}