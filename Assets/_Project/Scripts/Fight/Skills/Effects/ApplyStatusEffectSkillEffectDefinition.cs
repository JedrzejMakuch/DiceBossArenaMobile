using UnityEngine;

[CreateAssetMenu(
    fileName = "Effect_ApplyStatus_",
    menuName =
        "Dice Boss Arena/Skills/Effects/Apply Status Effect")]
public sealed class
    ApplyStatusEffectSkillEffectDefinition :
        SkillEffectDefinition
{
    [SerializeField]
    private StatusEffectDefinition statusEffect;

    [SerializeField]
    private StatusEffectTargetRelation targetRelation =
        StatusEffectTargetRelation.Enemy;

    public StatusEffectDefinition StatusEffect =>
        statusEffect;

    public StatusEffectTargetRelation TargetRelation =>
        targetRelation;

    public override bool CanApply(
        SkillExecutionContext context)
    {
        if (context == null ||
            context.Caster == null ||
            statusEffect == null ||
            !statusEffect.StatusEffectId.IsValid)
        {
            return false;
        }

        FightUnit target =
            context.PrimaryTarget;

        if (target == null ||
            !target.IsAlive)
        {
            return false;
        }

        return targetRelation switch
        {
            StatusEffectTargetRelation.AnyLivingUnit =>
                true,

            StatusEffectTargetRelation.Self =>
                target == context.Caster,

            StatusEffectTargetRelation.Ally =>
                target != context.Caster &&
                context.Caster.IsAlliedWith(target),

            StatusEffectTargetRelation.Enemy =>
                context.Caster.IsHostileTo(target),

            _ =>
                false
        };
    }

    public override void Apply(
        SkillExecutionContext context)
    {
        if (!CanApply(context))
        {
            return;
        }

        context.PrimaryTarget.ApplyStatusEffect(
            statusEffect);
    }

#if UNITY_EDITOR
    public void InitializeForTests(
        StatusEffectDefinition newStatusEffect,
        StatusEffectTargetRelation newTargetRelation =
            StatusEffectTargetRelation.Enemy)
    {
        statusEffect =
            newStatusEffect;

        targetRelation =
            newTargetRelation;
    }
#endif
}