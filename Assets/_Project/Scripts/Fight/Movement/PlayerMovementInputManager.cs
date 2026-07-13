using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovementInputManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightTurnResourceManager turnResourceManager;
    [SerializeField] private FightMovementManager movementManager;
    [SerializeField] private PlayerSkillSelectionManager skillSelectionManager;

    private readonly HashSet<FightGridTile> subscribedTiles = new();

    private bool playerMovementEnabled;

    private void OnEnable()
    {
        if (turnResourceManager != null)
        {
            turnResourceManager.TurnResourcesReady += HandleTurnResourcesReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded += HandleTurnEnded;
            turnManager.CombatStopped += HandleCombatStopped;
        }
    }

    private void OnDisable()
    {
        if (turnResourceManager != null)
        {
            turnResourceManager.TurnResourcesReady -= HandleTurnResourcesReady;
        }

        if (turnManager != null)
        {
            turnManager.TurnEnded -= HandleTurnEnded;
            turnManager.CombatStopped -= HandleCombatStopped;
        }

        UnsubscribeFromTiles();
    }

    private void HandleTurnResourcesReady(FightUnit unit)
    {
        EnsureTileSubscriptions();

        playerMovementEnabled =
        unit != null &&
        unit.IsAlive &&
        unit.IsControlledBy(
            FightControllerType.LocalPlayer);
    }

    private void HandleTurnEnded(FightUnit unit)
    {
        playerMovementEnabled = false;
    }

    private void HandleCombatStopped(string reason)
    {
        playerMovementEnabled = false;
    }

    private void HandleTileClicked(FightGridTile tile)
    {
        if (!playerMovementEnabled ||
            tile == null ||
            turnManager == null ||
            movementManager == null)
        {
            return;
        }

        FightUnit activeUnit = turnManager.ActiveUnit;

        if (activeUnit == null ||
        !activeUnit.IsControlledBy(
            FightControllerType.LocalPlayer))
        {
            return;
        }

        if (skillSelectionManager != null && skillSelectionManager.HasSelectedSkill)
        {
            UnitSkillState selectedSkill = skillSelectionManager.SelectedSkill;

            SkillTargetType targetType =
                selectedSkill?.Definition != null
                    ? selectedSkill.Definition.TargetType
                    : SkillTargetType.Self;

            if (targetType == SkillTargetType.Tile ||
                targetType == SkillTargetType.Area)
            {
                // Kliknięcie obsłuży
                // PlayerSkillTileInputManager.
                return;
            }

            skillSelectionManager.ClearSelection();

            Debug.Log(
                $"Skill selection cancelled by clicking tile " +
                $"({tile.GridX}, {tile.GridY}).",
                tile);

            return;
        }

        if (!movementManager.HighlightedTiles.Contains(tile))
        {
            Debug.Log(
                $"Tile ({tile.GridX}, {tile.GridY}) " +
                "is outside the current movement range.");

            return;
        }

        movementManager.TryMoveUnit(
            activeUnit,
            tile);
    }

    private void EnsureTileSubscriptions()
    {
        if (arenaGenerator == null)
        {
            return;
        }

        foreach (FightGridTile tile in arenaGenerator.GeneratedTiles)
        {
            if (tile == null ||
                subscribedTiles.Contains(tile))
            {
                continue;
            }

            tile.Clicked += HandleTileClicked;
            subscribedTiles.Add(tile);
        }
    }

    private void UnsubscribeFromTiles()
    {
        foreach (FightGridTile tile in subscribedTiles)
        {
            if (tile != null)
            {
                tile.Clicked -= HandleTileClicked;
            }
        }

        subscribedTiles.Clear();
    }
}