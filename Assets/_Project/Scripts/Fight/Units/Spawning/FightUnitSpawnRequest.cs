using UnityEngine;

public sealed class FightUnitSpawnRequest
{
    public FightUnit Prefab { get; }
    public FightUnitDefinition Definition { get; }
    public FightUnitOwnership Ownership { get; }
    public FightGridTile Tile { get; }
    public Transform Parent { get; }
    public string ObjectName { get; }

    public FightUnitSpawnRequest(
        FightUnit prefab,
        FightUnitDefinition definition,
        FightUnitOwnership ownership,
        FightGridTile tile,
        Transform parent = null,
        string objectName = null)
    {
        Prefab = prefab;
        Definition = definition;
        Ownership = ownership;
        Tile = tile;
        Parent = parent;
        ObjectName = objectName;
    }
}