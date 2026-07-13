using UnityEngine;

[CreateAssetMenu(fileName = "Effect_Damage_", menuName = "Dice Boss Arena/Skills/Effects/Damage")]
public class DamageSkillEffectDefinition : SkillEffectDefinition
{
    [Header("Base Damage")]
    [SerializeField] private DamageScalingSource scalingSource = DamageScalingSource.CasterAttackPower;

    [SerializeField, Min(0)] private int baseValue;

    [SerializeField, Min(0f)] private float scalingMultiplier = 1f;

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
            context.Caster == null)
        {
            return false;
        }

        FightUnit target = context.PrimaryTarget;

        if (target == null ||
            !target.IsAlive ||
            target == context.Caster)
        {
            return false;
        }

        return context.Caster.IsHostileTo(target);
    }

    public override void Apply(
        SkillExecutionContext context)
    {
        if (!CanApply(context))
        {
            return;
        }

        int damage = CalculateDamage(context);

        context.PrimaryTarget.TakeDamage(damage);

        Debug.Log(
            $"{context.Caster.UnitName} used " +
            $"{context.Skill.DisplayName} on " +
            $"{context.PrimaryTarget.UnitName}. " +
            $"Damage: {damage}.");
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
            GetScalingValue(context.Caster);

        float rawDamage =
            baseValue +
            scalingValue * scalingMultiplier;

        rawDamage *=
            context.LevelData.PowerMultiplier;

        rawDamage +=
            context.LevelData.FlatValueBonus;

        return Mathf.Max(
            0,
            Mathf.RoundToInt(rawDamage));
    }

    private float GetScalingValue(FightUnit caster)
    {
        return scalingSource switch
        {
            DamageScalingSource.FixedValue => 0f,

            DamageScalingSource.CasterAttackPower =>
                caster.AttackPower,

            _ => 0f
        };
    }
}