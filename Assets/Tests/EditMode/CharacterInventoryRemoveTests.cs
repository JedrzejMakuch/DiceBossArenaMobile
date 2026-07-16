using DiceBossArena.Game;
using NUnit.Framework;

public class CharacterInventoryRemoveTests
{
    [Test]
    public void TryRemove_FullQuantityRemovesSlot()
    {
        CharacterInventory inventory =
            CreateInventory(
                CreateItem(
                    "instance_001",
                    3));

        InventoryRemoveResult result =
            inventory.TryRemove(
                new CharacterItemInstanceId(
                    "instance_001"),
                3);

        Assert.That(
            result,
            Is.EqualTo(
                InventoryRemoveResult.Removed));

        Assert.That(
            inventory.Items,
            Is.Empty);

        Assert.That(
            inventory.IsFull,
            Is.False);
    }

    [Test]
    public void TryRemove_PartialQuantityReducesStack()
    {
        CharacterInventory inventory =
            CreateInventory(
                CreateItem(
                    "instance_001",
                    5));

        InventoryRemoveResult result =
            inventory.TryRemove(
                new CharacterItemInstanceId(
                    "instance_001"),
                2);

        Assert.That(
            result,
            Is.EqualTo(
                InventoryRemoveResult
                    .QuantityReduced));

        Assert.That(
            inventory.Count,
            Is.EqualTo(1));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(3));

        Assert.That(
            inventory.Items[0]
                .InstanceId.Value,
            Is.EqualTo("instance_001"));
    }

    [Test]
    public void TryRemove_MissingItemDoesNotChangeInventory()
    {
        CharacterInventory inventory =
            CreateInventory(
                CreateItem(
                    "instance_001",
                    3));

        InventoryRemoveResult result =
            inventory.TryRemove(
                new CharacterItemInstanceId(
                    "missing_instance"),
                1);

        Assert.That(
            result,
            Is.EqualTo(
                InventoryRemoveResult
                    .ItemNotFound));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(3));
    }

    [Test]
    public void TryRemove_TooLargeQuantityIsAtomic()
    {
        CharacterInventory inventory =
            CreateInventory(
                CreateItem(
                    "instance_001",
                    3));

        InventoryRemoveResult result =
            inventory.TryRemove(
                new CharacterItemInstanceId(
                    "instance_001"),
                4);

        Assert.That(
            result,
            Is.EqualTo(
                InventoryRemoveResult
                    .InvalidQuantity));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(3));
    }

    [Test]
    public void TryRemove_ZeroQuantityReturnsFailure()
    {
        CharacterInventory inventory =
            CreateInventory(
                CreateItem(
                    "instance_001",
                    3));

        InventoryRemoveResult result =
            inventory.TryRemove(
                new CharacterItemInstanceId(
                    "instance_001"),
                0);

        Assert.That(
            result,
            Is.EqualTo(
                InventoryRemoveResult
                    .InvalidQuantity));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(3));
    }

    [Test]
    public void TryRemove_InvalidInstanceIdReturnsFailure()
    {
        CharacterInventory inventory =
            CreateInventory(
                CreateItem(
                    "instance_001",
                    3));

        InventoryRemoveResult result =
            inventory.TryRemove(
                new CharacterItemInstanceId(" "),
                1);

        Assert.That(
            result,
            Is.EqualTo(
                InventoryRemoveResult
                    .InvalidInstanceId));

        Assert.That(
            inventory.Items[0].Quantity,
            Is.EqualTo(3));
    }

    private static CharacterInventory CreateInventory(
        params CharacterItemInstance[] items)
    {
        return new CharacterInventory(
            5,
            new ItemDefinitionCatalog(null),
            items);
    }

    private static CharacterItemInstance CreateItem(
        string instanceId,
        int quantity)
    {
        return new CharacterItemInstance(
            new CharacterItemInstanceId(
                instanceId),
            new CharacterItemId(
                "health_potion"),
            1,
            0,
            quantity);
    }
}