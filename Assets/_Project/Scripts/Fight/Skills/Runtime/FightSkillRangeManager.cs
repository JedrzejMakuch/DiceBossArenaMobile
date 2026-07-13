using System.Collections.Generic;
using UnityEngine;

public class FightSkillRangeManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField]
    private PlayerSkillSelectionManager skillSelectionManager;
    [SerializeField] private FightMovementManager movementManager;

    private readonly List<FightGridTile> highlightedTiles = new();

    public IReadOnlyList<FightGridTile> HighlightedTiles =>
        highlightedTiles;

    private void OnEnable()
    {
        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected += HandleSkillSelected;

            skillSelectionManager.SkillSelectionCleared += HandleSkillSelectionCleared;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped += HandleCombatStopped;
        }
    }

    private void OnDisable()
    {
        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected -= HandleSkillSelected;

            skillSelectionManager.SkillSelectionCleared -= HandleSkillSelectionCleared;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -= HandleCombatStopped;
        }

        ClearSkillRange();
    }

    private void HandleSkillSelected(
    FightUnit caster,
    UnitSkillState skillState)
    {
        SkillDefinition definition =
            skillState?.Definition;

        if (definition == null)
        {
            ClearSkillRange();
            return;
        }

        if (definition.RangeShape ==
            SkillRangeShape.Cone)
        {
            // Stożek potrzebuje wyboru kierunku.
            // Obsługuje go PlayerConePreviewManager.
            return;
        }

        ShowSkillRange(
            caster,
            skillState);
    }

    private void HandleSkillSelectionCleared()
    {
        ClearSkillRange();
        RestoreMovementRange();
    }

    private void HandleTurnEnded(FightUnit unit)
    {
        ClearSkillRange();
    }

    private void HandleCombatStopped(string reason)
    {
        ClearSkillRange();
    }

    public void ShowSkillRange(
        FightUnit caster,
        UnitSkillState skillState)
    {
        ClearSkillRange();

        if (caster == null ||
            caster.CurrentTile == null ||
            skillState == null ||
            skillState.Definition == null ||
            arenaGenerator == null)
        {
            return;
        }

        if (movementManager != null)
        {
            movementManager.ClearMovementRange();
        }

        SkillDefinition definition =
            skillState.Definition;

        foreach (FightGridTile tile in
                 arenaGenerator.GeneratedTiles)
        {
            if (!IsTileWithinSkillRange(
                    caster.CurrentTile,
                    tile,
                    definition))
            {
                continue;
            }

            tile.SetSkillRangeVisual();
            highlightedTiles.Add(tile);
        }

        Debug.Log(
            $"{caster.UnitName} skill range shown for " +
            $"{definition.DisplayName}. " +
            $"Range: {definition.MinRange}-" +
            $"{definition.MaxRange}, " +
            $"tiles: {highlightedTiles.Count}.",
            caster);
    }

    public bool IsTileInCurrentRange(
        FightGridTile tile)
    {
        return tile != null &&
               highlightedTiles.Contains(tile);
    }

    public bool IsUnitInCurrentRange(
        FightUnit unit)
    {
        return unit != null &&
               unit.CurrentTile != null &&
               IsTileInCurrentRange(unit.CurrentTile);
    }

    public void ClearSkillRange()
    {
        foreach (FightGridTile tile in highlightedTiles)
        {
            if (tile != null)
            {
                tile.SetNormalVisual();
            }
        }

        highlightedTiles.Clear();
    }

    private bool IsTileWithinSkillRange(
    FightGridTile origin,
    FightGridTile target,
    SkillDefinition definition)
    {
        return SkillRangeUtility.IsWithinRange(
            origin,
            target,
            definition);
    }

    private void RestoreMovementRange()
    {
        if (movementManager == null ||
            turnManager == null ||
            !turnManager.CombatRunning)
        {
            return;
        }

        FightUnit activeUnit =
            turnManager.ActiveUnit;

        if (!FightMovementManager
        .CanShowLocalMovementRange(activeUnit))
        {
            return;
        }

        FightUnitTurnResources resources =
            activeUnit.TurnResources;

        if (resources == null ||
            resources.CurrentMovementPoints <= 0)
        {
            return;
        }

        movementManager.ShowMovementRange(activeUnit);
    }
}