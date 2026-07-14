using UnityEngine;

public sealed class FightActionExecutor :
    MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private FightTurnManager turnManager;

    [SerializeField]
    private FightMovementManager movementManager;

    [SerializeField]
    private FightSkillManager skillManager;

    public bool TryExecute(
        IFightActionRequest request)
    {
        if (!CanExecuteRequest(request))
        {
            return false;
        }

        return request switch
        {
            FightMoveActionRequest moveRequest =>
                ExecuteMove(moveRequest),

            FightSkillActionRequest skillRequest =>
                ExecuteSkill(skillRequest),

            FightEndTurnActionRequest endTurnRequest =>
                ExecuteEndTurn(endTurnRequest),

            _ => false
        };
    }

    private bool CanExecuteRequest(
        IFightActionRequest request)
    {
        if (request == null ||
            request.Actor == null)
        {
            return false;
        }

        if (turnManager == null ||
            !turnManager.CombatRunning)
        {
            return false;
        }

        if (turnManager.ActiveUnit !=
            request.Actor)
        {
            return false;
        }

        if (!request.Actor.IsAlive)
        {
            return false;
        }

        return true;
    }

    private bool ExecuteMove(
        FightMoveActionRequest request)
    {
        if (movementManager == null ||
            request.TargetTile == null)
        {
            return false;
        }

        return movementManager.TryMoveUnit(
            request.Actor,
            request.TargetTile);
    }

    private bool ExecuteSkill(
        FightSkillActionRequest request)
    {
        if (skillManager == null ||
            request.SkillState == null)
        {
            return false;
        }

        return skillManager.TryExecuteSkill(
            request.Actor,
            request.SkillState,
            request.PrimaryTarget,
            request.TargetTile,
            request.AffectedUnits);
    }

    private bool ExecuteEndTurn(
        FightEndTurnActionRequest request)
    {
        turnManager.EndCurrentTurn();

        return true;
    }
}