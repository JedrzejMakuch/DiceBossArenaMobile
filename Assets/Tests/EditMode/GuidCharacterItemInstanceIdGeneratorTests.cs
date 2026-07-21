using DiceBossArena.Game;
using NUnit.Framework;

public sealed class
    GuidCharacterItemInstanceIdGeneratorTests
{
    [Test]
    public void Generate_ReturnsValidId()
    {
        GuidCharacterItemInstanceIdGenerator generator =
            new GuidCharacterItemInstanceIdGenerator();

        CharacterItemInstanceId result =
            generator.Generate();

        Assert.That(
            result.IsValid,
            Is.True);
    }

    [Test]
    public void Generate_ReturnsIdWithExpectedLength()
    {
        GuidCharacterItemInstanceIdGenerator generator =
            new GuidCharacterItemInstanceIdGenerator();

        CharacterItemInstanceId result =
            generator.Generate();

        Assert.That(
            result.Value.Length,
            Is.EqualTo(32));
    }

    [Test]
    public void Generate_TwiceReturnsDifferentIds()
    {
        GuidCharacterItemInstanceIdGenerator generator =
            new GuidCharacterItemInstanceIdGenerator();

        CharacterItemInstanceId first =
            generator.Generate();

        CharacterItemInstanceId second =
            generator.Generate();

        Assert.That(
            second,
            Is.Not.EqualTo(first));
    }
}