using System.Collections.Generic;

public sealed class FightSkillActionRequest :
    IFightActionRequest
{
    public FightActionType ActionType =>
        FightActionType.Skill;

    public FightUnit Actor { get; }

    public UnitSkillState SkillState { get; }

    public FightUnit PrimaryTarget { get; }

    public FightGridTile TargetTile { get; }

    public IReadOnlyList<FightUnit> AffectedUnits { get; }

    public FightSkillActionRequest(
        FightUnit actor,
        UnitSkillState skillState,
        FightUnit primaryTarget = null,
        FightGridTile targetTile = null,
        IReadOnlyList<FightUnit> affectedUnits = null)
    {
        Actor = actor;
        SkillState = skillState;
        PrimaryTarget = primaryTarget;
        TargetTile = targetTile;
        AffectedUnits = affectedUnits;
    }
}