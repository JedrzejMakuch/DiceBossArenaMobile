using UnityEngine;

[CreateAssetMenu(
    fileName = "Effect_AreaDamage_",
    menuName =
        "Dice Boss Arena/Skills/Effects/Area Damage")]
public class AreaDamageSkillEffectDefinition :
    SkillEffectDefinition
{
    [Header("Base Damage")]
    [SerializeField]
    private DamageScalingSource scalingSource =
        DamageScalingSource.CasterAttackPower;

    [SerializeField, Min(0)]
    private int baseValue;

    [SerializeField, Min(0f)]
    private float scalingMultiplier = 1f;

    public DamageScalingSource ScalingSource =>
        scalingSource;

    public int BaseValue =>
        baseValue;

    public float ScalingMultiplier =>
        scalingMultiplier;

    public override bool CanApply(
        SkillExecutionContext context)
    {
        if (context == null ||
            context.Caster == null ||
            context.Skill == null)
        {
            return false;
        }

        if (!context.Caster.IsAlive)
        {
            return false;
        }

        return context.AffectedUnits != null;
    }

    public override void Apply(
        SkillExecutionContext context)
    {
        if (!CanApply(context))
        {
            return;
        }

        int damage =
            CalculateDamage(context);

        int damagedUnits = 0;

        foreach (FightUnit unit in
                 context.AffectedUnits)
        {
            if (unit == null ||
                !unit.IsAlive ||
                !context.Caster.IsHostileTo(unit))
            {
                continue;
            }

            unit.TakeDamage(damage);
            damagedUnits++;

            Debug.Log(
                $"{context.Caster.UnitName} used " +
                $"{context.Skill.DisplayName} on " +
                $"{unit.UnitName}. Damage: {damage}.",
                unit);
        }

        Debug.Log(
            $"{context.Skill.DisplayName} affected " +
            $"{damagedUnits} hostile units.",
            context.Caster);
    }

    public int CalculateDamage(
        SkillExecutionContext context)
    {
        if (context == null ||
            context.Caster == null)
        {
            return 0;
        }

        float scalingValue =
            GetScalingValue(
                context.Caster);

        float rawDamage =
            baseValue +
            scalingValue *
            scalingMultiplier;

        rawDamage *=
            context.LevelData.PowerMultiplier;

        rawDamage +=
            context.LevelData.FlatValueBonus;

        return Mathf.Max(
            0,
            Mathf.RoundToInt(rawDamage));
    }

    private float GetScalingValue(
        FightUnit caster)
    {
        return scalingSource switch
        {
            DamageScalingSource.FixedValue =>
                0f,

            DamageScalingSource
                .CasterAttackPower =>
                caster.AttackPower,

            _ => 0f
        };
    }
}