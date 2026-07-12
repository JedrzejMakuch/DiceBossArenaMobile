using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillTileInputManager :
    MonoBehaviour
{
    [Header("References")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightTurnManager turnManager;
    [SerializeField] private FightSkillManager skillManager;
    [SerializeField] private PlayerSkillSelectionManager skillSelectionManager;
    [SerializeField] private FightSkillRangeManager skillRangeManager;

    private readonly HashSet<FightGridTile> subscribedTiles = new();

    private void OnEnable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted +=
                HandleTurnStarted;

            turnManager.CombatStopped +=
                HandleCombatStopped;
        }

        EnsureTileSubscriptions();
    }

    private void OnDisable()
    {
        if (turnManager != null)
        {
            turnManager.TurnStarted -=
                HandleTurnStarted;

            turnManager.CombatStopped -=
                HandleCombatStopped;
        }

        UnsubscribeFromTiles();
    }

    private void HandleTurnStarted(
        FightUnit unit,
        int roundNumber)
    {
        EnsureTileSubscriptions();
    }

    private void HandleCombatStopped(
        string reason)
    {
        UnsubscribeFromTiles();
    }

    private void HandleTileClicked(
        FightGridTile tile)
    {
        if (tile == null ||
            turnManager == null ||
            skillManager == null ||
            skillSelectionManager == null ||
            skillRangeManager == null)
        {
            return;
        }

        if (!skillSelectionManager.HasSelectedSkill)
        {
            return;
        }

        FightUnit caster =
            turnManager.ActiveUnit;

        if (caster == null ||
            !caster.IsAlive ||
            caster.Team != FightTeam.Player)
        {
            return;
        }

        if (skillSelectionManager.SelectedCaster !=
            caster)
        {
            skillSelectionManager.ClearSelection();
            return;
        }

        UnitSkillState selectedSkill =
            skillSelectionManager.SelectedSkill;

        if (selectedSkill == null ||
            selectedSkill.Definition == null)
        {
            return;
        }

        SkillTargetType targetType =
            selectedSkill.Definition.TargetType;

        if (targetType != SkillTargetType.Tile)
        {
            return;
        }

        if (!skillRangeManager
                .IsTileInCurrentRange(tile))
        {
            Debug.Log(
                $"Tile ({tile.GridX}, {tile.GridY}) " +
                $"is outside the selected skill range.",
                tile);

            skillSelectionManager.ClearSelection();
            return;
        }

        bool executed =
            skillManager.TryExecuteSkill(
                caster,
                selectedSkill,
                primaryTarget: null,
                targetTile: tile);

        if (!executed)
        {
            Debug.Log(
                $"Tile skill could not be executed on " +
                $"({tile.GridX}, {tile.GridY}).",
                tile);

            return;
        }

        skillSelectionManager.ClearSelection();
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

            tile.Clicked += HandleTileClicked;
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