using UnityEngine;

[CreateAssetMenu(fileName = "Effect_Heal_", menuName = "Dice Boss Arena/Skills/Effects/Healing")]
public class HealingSkillEffectDefinition : SkillEffectDefinition
{
    [Header("Healing")]
    [SerializeField, Min(0)] private int baseHeal = 4;

    public int BaseHeal => baseHeal;

    public override bool CanApply(
        SkillExecutionContext context)
    {
        if (context == null ||
            context.Caster == null)
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

        return context.Caster
            .IsAlliedWith(target);
    }

    public override void Apply(
        SkillExecutionContext context)
    {
        if (!CanApply(context))
        {
            return;
        }

        int healAmount =
            CalculateHealing(context);

        int healthBefore =
            context.PrimaryTarget.CurrentHealth;

        context.PrimaryTarget.Heal(
            healAmount);

        int actualHealing =
            context.PrimaryTarget.CurrentHealth -
            healthBefore;

        Debug.Log(
            $"{context.Caster.UnitName} used " +
            $"{context.Skill.DisplayName} on " +
            $"{context.PrimaryTarget.UnitName}. " +
            $"Healing: {actualHealing}.",
            context.PrimaryTarget);
    }

    public int CalculateHealing(
        SkillExecutionContext context)
    {
        if (context == null)
        {
            return 0;
        }

        float rawHealing =
            baseHeal *
            context.LevelData.PowerMultiplier;

        rawHealing +=
            context.LevelData.FlatValueBonus;

        return Mathf.Max(
            0,
            Mathf.RoundToInt(rawHealing));
    }
}