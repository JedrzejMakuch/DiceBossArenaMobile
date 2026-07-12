using System.Collections.Generic;

public class SkillExecutionContext
{
    public SkillDefinition Skill { get; }
    public SkillLevelData LevelData { get; }
    public int SkillLevel { get; }

    public FightUnit Caster { get; }
    public FightUnit PrimaryTarget { get; }
    public FightGridTile TargetTile { get; }

    public IReadOnlyList<FightUnit> AffectedUnits { get; }

    public SkillExecutionContext(
        SkillDefinition skill,
        int skillLevel,
        FightUnit caster,
        FightUnit primaryTarget,
        FightGridTile targetTile,
        IReadOnlyList<FightUnit> affectedUnits)
    {
        Skill = skill;
        SkillLevel = skillLevel;
        LevelData = skill != null
            ? skill.GetLevelData(skillLevel)
            : SkillLevelData.Default;

        Caster = caster;
        PrimaryTarget = primaryTarget;
        TargetTile = targetTile;
        AffectedUnits =
            affectedUnits ?? new List<FightUnit>();
    }
}