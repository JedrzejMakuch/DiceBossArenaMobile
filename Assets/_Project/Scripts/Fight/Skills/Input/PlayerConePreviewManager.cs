using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConePreviewManager :
    MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private FightArenaGenerator arenaGenerator;

    [SerializeField]
    private FightTurnManager turnManager;

    [SerializeField]
    private PlayerSkillSelectionManager
        skillSelectionManager;

    [SerializeField]
    private FightSkillRangeManager
    skillRangeManager;

    [SerializeField]
    private FightMovementManager
    movementManager;

    private readonly HashSet<FightGridTile>
        subscribedTiles = new();

    private readonly List<FightGridTile>
        directionSelectorTiles = new();

    private readonly List<FightGridTile>
        conePreviewTiles = new();

    private FightGridTile selectedDirectionTile;
    private SkillDirection selectedDirection =
        SkillDirection.None;

    private bool coneConfirmed;

    public SkillDirection SelectedDirection =>
        selectedDirection;

    public FightGridTile SelectedDirectionTile =>
        selectedDirectionTile;

    public IReadOnlyList<FightGridTile>
        ConePreviewTiles => conePreviewTiles;

    public bool ConeConfirmed =>
        coneConfirmed;

    public event Action<
        FightUnit,
        UnitSkillState,
        SkillDirection,
        IReadOnlyList<FightGridTile>>
        ConeSelectionConfirmed;

    private void OnEnable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted +=
                HandleTurnStarted;

            turnManager.TurnEnded +=
                HandleTurnEnded;

            turnManager.CombatStopped +=
                HandleCombatStopped;
        }

        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected +=
                HandleSkillSelected;

            skillSelectionManager
                .SkillSelectionCleared +=
                HandleSkillSelectionCleared;
        }

        EnsureTileSubscriptions();
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted -=
                HandleTurnStarted;

            turnManager.TurnEnded -=
                HandleTurnEnded;

            turnManager.CombatStopped -=
                HandleCombatStopped;
        }

        if (skillSelectionManager != null)
        {
            skillSelectionManager.SkillSelected -=
                HandleSkillSelected;

            skillSelectionManager
                .SkillSelectionCleared -=
                HandleSkillSelectionCleared;
        }

        ClearConeSelection();
        UnsubscribeFromTiles();
    }

    private void HandleTurnStarted(
        FightUnit unit,
        int roundNumber)
    {
        EnsureTileSubscriptions();
        ClearConeSelection();
    }

    private void HandleTurnEnded(
        FightUnit unit)
    {
        ClearConeSelection();
    }

    private void HandleCombatStopped(
        string reason)
    {
        ClearConeSelection();
    }

    private void HandleSkillSelected(
    FightUnit caster,
    UnitSkillState skillState)
    {
        ClearConeSelection();

        SkillDefinition definition =
            skillState?.Definition;

        if (!PlayerSkillSelectionManager
        .CanUseLocalSkillSelection(caster) ||
        caster.CurrentTile == null ||
        definition == null ||
        definition.RangeShape !=
            SkillRangeShape.Cone)
        {
            return;
        }

        if (skillRangeManager != null)
        {
            skillRangeManager.ClearSkillRange();
        }

        if (movementManager != null)
        {
            movementManager.ClearMovementRange();
        }

        ShowDirectionSelectors(
            caster.CurrentTile);
    }

    private void HandleSkillSelectionCleared()
    {
        ClearConeSelection();
    }

    private void HandleTileClicked(
        FightGridTile tile)
    {
        if (tile == null ||
            turnManager == null ||
            skillSelectionManager == null ||
            arenaGenerator == null)
        {
            return;
        }

        if (!skillSelectionManager.HasSelectedSkill)
        {
            return;
        }

        UnitSkillState selectedSkill =
            skillSelectionManager.SelectedSkill;

        SkillDefinition definition =
            selectedSkill?.Definition;

        if (definition == null ||
            definition.RangeShape !=
            SkillRangeShape.Cone)
        {
            return;
        }

        FightUnit caster =
            turnManager.ActiveUnit;

        if (!PlayerSkillSelectionManager
        .CanUseLocalSkillSelection(caster) ||
            caster.CurrentTile == null)
        {
            return;
        }

        if (!directionSelectorTiles.Contains(tile))
        {
            Debug.Log(
                $"Tile ({tile.GridX}, {tile.GridY}) " +
                $"is not a valid cone direction. " +
                $"Ground Slam selection cancelled.",
                tile);

            skillSelectionManager.ClearSelection();
            return;
        }

        SkillDirection clickedDirection =
            SkillRangeUtility.GetDirection(
                caster.CurrentTile,
                tile);

        if (clickedDirection ==
            SkillDirection.None)
        {
            return;
        }

        bool clickedSelectedDirection =
            selectedDirectionTile == tile &&
            selectedDirection ==
            clickedDirection;

        if (clickedSelectedDirection)
        {
            ConfirmCone(
                caster,
                selectedSkill);

            return;
        }

        SelectDirection(
            caster.CurrentTile,
            tile,
            clickedDirection,
            definition.MaxRange);
    }

    private void ShowDirectionSelectors(
        FightGridTile origin)
    {
        if (origin == null ||
            arenaGenerator == null)
        {
            return;
        }

        foreach (FightGridTile tile in
                 arenaGenerator.GeneratedTiles)
        {
            if (!SkillRangeUtility
                    .IsDirectionSelectorTile(
                        origin,
                        tile))
            {
                continue;
            }

            tile.SetMovementRangeVisual();
            directionSelectorTiles.Add(tile);
        }

        Debug.Log(
            $"Cone direction selectors shown: " +
            $"{directionSelectorTiles.Count}.");
    }

    private void SelectDirection(
        FightGridTile origin,
        FightGridTile directionTile,
        SkillDirection direction,
        int maxRange)
    {
        ClearConePreview();

        selectedDirectionTile =
            directionTile;

        selectedDirection =
            direction;

        coneConfirmed = false;

        RefreshDirectionSelectorVisuals();

        foreach (FightGridTile tile in
                 arenaGenerator.GeneratedTiles)
        {
            if (!SkillRangeUtility.IsTileInCone(
                    origin,
                    tile,
                    direction,
                    maxRange))
            {
                continue;
            }

            tile.SetSkillRangeVisual();
            conePreviewTiles.Add(tile);
        }

        // Pole kierunku ma pozostać żółte,
        // nawet jeżeli znajduje się również w stożku.
        if (selectedDirectionTile != null)
        {
            selectedDirectionTile
                .SetSelectedVisual();
        }

        Debug.Log(
            $"Cone direction selected: {direction}. " +
            $"Tap the same direction tile again " +
            $"to confirm. " +
            $"Tiles: {conePreviewTiles.Count}.");
    }

    private void ConfirmCone(
        FightUnit caster,
        UnitSkillState skillState)
    {
        if (caster == null ||
            skillState == null ||
            selectedDirection ==
            SkillDirection.None ||
            conePreviewTiles.Count == 0)
        {
            return;
        }

        coneConfirmed = true;

        Debug.Log(
            $"Cone confirmed: {selectedDirection}. " +
            $"Affected tiles: {conePreviewTiles.Count}.",
            caster);

        ConeSelectionConfirmed?.Invoke(
            caster,
            skillState,
            selectedDirection,
            conePreviewTiles);
    }

    private void RefreshDirectionSelectorVisuals()
    {
        foreach (FightGridTile tile in
                 directionSelectorTiles)
        {
            if (tile == null)
            {
                continue;
            }

            if (tile == selectedDirectionTile)
            {
                tile.SetSelectedVisual();
            }
            else
            {
                tile.SetMovementRangeVisual();
            }
        }
    }

    private void ClearConePreview()
    {
        foreach (FightGridTile tile in
                 conePreviewTiles)
        {
            if (tile != null)
            {
                tile.SetNormalVisual();
            }
        }

        conePreviewTiles.Clear();

        // Po wyczyszczeniu czerwonych pól
        // przywracamy wizuale selektorów.
        RefreshDirectionSelectorVisuals();
    }

    private void ClearConeSelection()
    {
        foreach (FightGridTile tile in
                 conePreviewTiles)
        {
            if (tile != null)
            {
                tile.SetNormalVisual();
            }
        }

        foreach (FightGridTile tile in
                 directionSelectorTiles)
        {
            if (tile != null)
            {
                tile.SetNormalVisual();
            }
        }

        conePreviewTiles.Clear();
        directionSelectorTiles.Clear();

        selectedDirectionTile = null;
        selectedDirection =
            SkillDirection.None;

        coneConfirmed = false;
    }

    private void EnsureTileSubscriptions()
    {
        if (arenaGenerator == null)
        {
            return;
        }

        foreach (FightGridTile tile in
                 arenaGenerator.GeneratedTiles)
        {
            if (tile == null ||
                subscribedTiles.Contains(tile))
            {
                continue;
            }

            tile.Clicked +=
                HandleTileClicked;

            subscribedTiles.Add(tile);
        }
    }

    private void UnsubscribeFromTiles()
    {
        foreach (FightGridTile tile in
                 subscribedTiles)
        {
            if (tile != null)
            {
                tile.Clicked -=
                    HandleTileClicked;
            }
        }

        subscribedTiles.Clear();
    }
}