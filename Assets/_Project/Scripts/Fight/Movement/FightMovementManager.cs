using System;
using System.Collections.Generic;
using UnityEngine;

public class FightMovementManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightTurnResourceManager turnResourceManager;

    private readonly List<FightGridTile> highlightedTiles = new();

    public IReadOnlyList<FightGridTile> HighlightedTiles => highlightedTiles;
    public event Action<FightUnit, FightGridTile, int> UnitMoved;

    private void OnEnable()
    {
        if (turnManager != null)
        {
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped += HandleCombatStopped;
        }

        if (turnResourceManager != null)
        {
            turnResourceManager.TurnResourcesReady += HandleTurnResourcesReady;
        }
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -= HandleCombatStopped;
        }

        if (turnResourceManager != null)
        {
            turnResourceManager.TurnResourcesReady -= HandleTurnResourcesReady;
        }
    }

    private void HandleTurnResourcesReady(FightUnit unit)
    {
        ClearMovementRange();

        if (unit == null ||
            !unit.IsAlive ||
            unit.Team != FightTeam.Player)
        {
            return;
        }

        ShowMovementRange(unit);
    }

    private void HandleTurnEnded(FightUnit unit)
    {
        ClearMovementRange();
    }

    private void HandleCombatStopped(string reason)
    {
        ClearMovementRange();
    }

    public void ShowMovementRange(FightUnit unit)
    {
        ClearMovementRange();

        if (unit == null || unit.CurrentTile == null)
        {
            return;
        }

        FightUnitTurnResources resources = unit.TurnResources;

        if (resources == null)
        {
            Debug.LogError(
                $"FightMovementManager: " +
                $"{unit.UnitName} has no FightUnitTurnResources.",
                unit);

            return;
        }

        List<FightGridTile> reachableTiles =
            GetReachableTiles(
                unit,
                resources.CurrentMovementPoints);

        foreach (FightGridTile tile in reachableTiles)
        {
            tile.SetMovementRangeVisual();
            highlightedTiles.Add(tile);
        }

        Debug.Log(
            $"{unit.UnitName} movement range shown. " +
            $"MP: {resources.CurrentMovementPoints}, " +
            $"reachable tiles: {highlightedTiles.Count}");
    }

    public List<FightGridTile> GetReachableTiles(
        FightUnit unit,
        int movementPoints)
    {
        List<FightGridTile> reachableTiles = new();

        if (unit == null ||
            unit.CurrentTile == null ||
            arenaGenerator == null ||
            movementPoints <= 0)
        {
            return reachableTiles;
        }

        foreach (FightGridTile tile in arenaGenerator.GeneratedTiles)
        {
            if (!CanUnitMoveToTile(
                    unit,
                    tile,
                    movementPoints))
            {
                continue;
            }

            reachableTiles.Add(tile);
        }

        return reachableTiles;
    }

    public bool CanUnitMoveToTile(
        FightUnit unit,
        FightGridTile tile,
        int availableMovementPoints)
    {
        if (unit == null ||
            unit.CurrentTile == null ||
            tile == null)
        {
            return false;
        }

        if (tile == unit.CurrentTile)
        {
            return false;
        }

        if (!tile.CanBeOccupiedBy(unit))
        {
            return false;
        }

        int movementCost =
            CalculateMovementCost(
                unit.CurrentTile,
                tile);

        return movementCost > 0 &&
               movementCost <= availableMovementPoints;
    }

    public int CalculateMovementCost(
        FightGridTile startTile,
        FightGridTile targetTile)
    {
        if (startTile == null || targetTile == null)
        {
            return int.MaxValue;
        }

        int distanceX =
            Mathf.Abs(startTile.GridX - targetTile.GridX);

        int distanceY =
            Mathf.Abs(startTile.GridY - targetTile.GridY);

        return distanceX + distanceY;
    }

    public void ClearMovementRange()
    {
        foreach (FightGridTile tile in highlightedTiles)
        {
            if (tile == null)
            {
                continue;
            }

            tile.SetNormalVisual();
        }

        highlightedTiles.Clear();
    }

    public bool TryMoveUnit(
    FightUnit unit,
    FightGridTile targetTile)
    {
        if (unit == null ||
            targetTile == null ||
            unit.CurrentTile == null)
        {
            return false;
        }

        if (turnManager == null ||
            !turnManager.CombatRunning ||
            turnManager.ActiveUnit != unit)
        {
            Debug.LogWarning(
                $"FightMovementManager: {unit.UnitName} " +
                $"cannot move outside its active turn.",
                unit);

            return false;
        }

        FightUnitTurnResources resources = unit.TurnResources;

        if (resources == null)
        {
            Debug.LogError(
                $"FightMovementManager: " +
                $"{unit.UnitName} has no FightUnitTurnResources.",
                unit);

            return false;
        }

        int movementCost =
            CalculateMovementCost(
                unit.CurrentTile,
                targetTile);

        if (!CanUnitMoveToTile(
                unit,
                targetTile,
                resources.CurrentMovementPoints))
        {
            Debug.LogWarning(
                $"{unit.UnitName} cannot move to tile " +
                $"({targetTile.GridX}, {targetTile.GridY}). " +
                $"Cost: {movementCost}, " +
                $"available MP: {resources.CurrentMovementPoints}.",
                unit);

            return false;
        }

        if (!unit.TryAssignToTile(targetTile))
        {
            return false;
        }

        if (!resources.TrySpendMovementPoints(movementCost))
        {
            Debug.LogError(
                $"FightMovementManager: failed to spend " +
                $"{movementCost} MP after moving {unit.UnitName}.",
                unit);

            return false;
        }

        UnitMoved?.Invoke(
            unit,
            targetTile,
            movementCost);

        Debug.Log(
            $"{unit.UnitName} moved to tile " +
            $"({targetTile.GridX}, {targetTile.GridY}). " +
            $"Cost: {movementCost} MP. " +
            $"Remaining: {resources.CurrentMovementPoints} MP.");

        ShowMovementRange(unit);

        return true;
    }
}