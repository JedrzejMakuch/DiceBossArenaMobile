using UnityEngine;

public sealed class FightUnitSpawner : MonoBehaviour
{
    [SerializeField] private FightUnitRegistry unitRegistry;

    public FightUnit Spawn(FightUnitSpawnRequest request)
    {
        if (!IsValid(request))
        {
            return null;
        }

        FightUnit unit = Instantiate(
            request.Prefab,
            request.Parent);

        if (!string.IsNullOrWhiteSpace(request.ObjectName))
        {
            unit.name = request.ObjectName;
        }

        if (!unit.Initialize(
                request.Definition,
                request.Ownership))
        {
            DestroySpawnedUnit(unit);
            return null;
        }

        if (!unit.TryAssignToTile(request.Tile))
        {
            DestroySpawnedUnit(unit);
            return null;
        }

        if (!unitRegistry.Register(unit))
        {
            unit.ReleaseCurrentTile();
            DestroySpawnedUnit(unit);
            return null;
        }

        return unit;
    }

    private bool IsValid(FightUnitSpawnRequest request)
    {
        if (request == null)
        {
            Debug.LogError(
                "FightUnitSpawner cannot spawn from a null request.",
                this);

            return false;
        }

        if (unitRegistry == null)
        {
            Debug.LogError(
                "FightUnitSpawner requires a FightUnitRegistry.",
                this);

            return false;
        }

        if (request.Prefab == null)
        {
            Debug.LogError(
                "FightUnitSpawnRequest requires a prefab.",
                this);

            return false;
        }

        if (request.Definition == null)
        {
            Debug.LogError(
                "FightUnitSpawnRequest requires a definition.",
                this);

            return false;
        }

        if (request.Ownership == null)
        {
            Debug.LogError(
                "FightUnitSpawnRequest requires ownership.",
                this);

            return false;
        }

        if (request.Tile == null)
        {
            Debug.LogError(
                "FightUnitSpawnRequest requires a tile.",
                this);

            return false;
        }

        return true;
    }

    private static void DestroySpawnedUnit(FightUnit unit)
    {
        if (unit == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            Destroy(unit.gameObject);
        }
        else
        {
            DestroyImmediate(unit.gameObject);
        }
    }

    public void Despawn(FightUnit unit)
    {
        if (unit == null)
        {
            return;
        }

        if (unitRegistry != null)
        {
            unitRegistry.Unregister(unit);
        }

        unit.ReleaseCurrentTile();
        DestroySpawnedUnit(unit);
    }
}