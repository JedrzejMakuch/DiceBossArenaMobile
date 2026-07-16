namespace DiceBossArena.Game
{
    public enum EquipmentOperationResult
    {
        Equipped = 0,
        Unequipped = 1,
        Swapped = 2,
        ItemNotFound = 3,
        UnknownDefinition = 4,
        InvalidSlot = 5,
        RequirementsNotMet = 6,
        SlotOccupied = 7,
        LoadoutConflict = 8,
        SlotEmpty = 9,
        AlreadyEquipped = 10
    }
}