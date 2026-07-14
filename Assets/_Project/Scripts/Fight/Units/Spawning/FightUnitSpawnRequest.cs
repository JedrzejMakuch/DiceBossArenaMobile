using DiceBossArena.Game;
using UnityEngine;

public sealed class FightUnitSpawnRequest
{
    public FightUnit Prefab { get; }

    public FightUnitDefinition Definition { get; }

    public FightUnitOwnership Ownership { get; }

    public FightGridTile Tile { get; }

    public Transform Parent { get; }

    public string ObjectName { get; }

    public FightUnitRuntimeSnapshot RuntimeSnapshot { get; }

    public CharacterBuildSnapshot BuildSnapshot { get; }

    public FightUnitSpawnRequest(
        FightUnit prefab,
        FightUnitDefinition definition,
        FightUnitOwnership ownership,
        FightGridTile tile,
        Transform parent = null,
        string objectName = null,
        FightUnitRuntimeSnapshot runtimeSnapshot = null,
        CharacterBuildSnapshot buildSnapshot = null)
    {
        Prefab = prefab;
        Definition = definition;
        Ownership = ownership;
        Tile = tile;
        Parent = parent;
        ObjectName = objectName;

        RuntimeSnapshot =
            runtimeSnapshot ??
            FightUnitRuntimeSnapshot.Fresh;

        BuildSnapshot =
            (buildSnapshot ??
             CharacterBuildSnapshot.Empty)
            .Copy();
    }
}