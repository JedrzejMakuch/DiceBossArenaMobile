using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlayerConeSkillExecutionManager :
    MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private PlayerConePreviewManager
        conePreviewManager;

    [SerializeField]
    private FightSkillManager
        skillManager;

    [SerializeField]
    private PlayerSkillSelectionManager
        skillSelectionManager;

    [SerializeField]
    private FightTurnManager
        turnManager;

    private void OnEnable()
    {
        if (conePreviewManager != null)
        {
            conePreviewManager
                .ConeSelectionConfirmed +=
                HandleConeSelectionConfirmed;
        }
    }

    private void OnDisable()
    {
        if (conePreviewManager != null)
        {
            conePreviewManager
                .ConeSelectionConfirmed -=
                HandleConeSelectionConfirmed;
        }
    }

    private void HandleConeSelectionConfirmed(
        FightUnit caster,
        UnitSkillState skillState,
        SkillDirection direction,
        IReadOnlyList<FightGridTile>
            coneTiles)
    {
        if (caster == null ||
            skillState == null ||
            skillState.Definition == null ||
            coneTiles == null ||
            skillManager == null ||
            skillSelectionManager == null ||
            turnManager == null)
        {
            return;
        }

        if (!turnManager.CombatRunning ||
        turnManager.ActiveUnit != caster ||
        !PlayerSkillSelectionManager
            .CanUseLocalSkillSelection(caster))
        {
            return;
        }

        SkillDefinition definition =
            skillState.Definition;

        if (definition.TargetType !=
                SkillTargetType.Area ||
            definition.RangeShape !=
                SkillRangeShape.Cone)
        {
            return;
        }

        FightGridTile directionTile =
            conePreviewManager
                .SelectedDirectionTile;

        if (directionTile == null)
        {
            return;
        }

        List<FightUnit> affectedUnits =
            CollectAffectedHostileUnits(
                caster,
                coneTiles);

        bool executed =
            skillManager.TryExecuteSkill(
                caster,
                skillState,
                primaryTarget: null,
                targetTile: directionTile,
                affectedUnits: affectedUnits);

        if (!executed)
        {
            Debug.LogWarning(
                $"{definition.DisplayName} could not " +
                $"be executed in direction " +
                $"{direction}.",
                caster);

            return;
        }

        Debug.Log(
            $"{definition.DisplayName} executed " +
            $"in direction {direction}. " +
            $"Units hit: " +
            $"{affectedUnits.Count}.",
            caster);

        StartCoroutine(ClearSelectionNextFrame());
    }

    private List<FightUnit> CollectAffectedHostileUnits(
            FightUnit caster,
            IReadOnlyList<FightGridTile>
                coneTiles)
    {
        List<FightUnit> affectedUnits =
            new();

        foreach (FightGridTile tile in
                 coneTiles)
        {
            if (tile == null ||
                !tile.IsOccupied)
            {
                continue;
            }

            FightUnit unit =
                tile.OccupyingUnit;

            if (unit == null ||
                !unit.IsAlive ||
                !caster.IsHostileTo(unit) ||
                affectedUnits.Contains(unit))
            {
                continue;
            }

            affectedUnits.Add(unit);
        }

        return affectedUnits;
    }

    private IEnumerator ClearSelectionNextFrame()
    {
        // To samo kliknięcie jest również wysyłane
        // do PlayerMovementInputManager.
        // Czekamy jedną klatkę, aby nie zostało
        // potraktowane jako zwykły ruch.
        yield return null;

        if (skillSelectionManager != null)
        {
            skillSelectionManager.ClearSelection();
        }
    }
}