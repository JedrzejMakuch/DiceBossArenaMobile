using UnityEngine;

[CreateAssetMenu(
    fileName = "StatusEffectBehavior_PeriodicDamage_",
    menuName =
        "Dice Boss Arena/Status Effects/Behaviors/Periodic Damage")]
public sealed class
    PeriodicDamageStatusEffectBehaviorDefinition :
        StatusEffectBehaviorDefinition
{
    [SerializeField, Min(0)]
    private int damagePerStack = 1;

    public int DamagePerStack =>
        damagePerStack;

    public int CalculateDamage(
        int stacks)
    {
        int normalizedStacks =
            Mathf.Max(
                1,
                stacks);

        return damagePerStack *
               normalizedStacks;
    }

    public override void Execute(
        StatusEffectExecutionContext context)
    {
        if (!CanExecute(context))
        {
            return;
        }

        int damage =
            CalculateDamage(
                context.Stacks);

        if (damage <= 0)
        {
            return;
        }

        context.Owner.TakeDamage(
            damage);
    }

#if UNITY_EDITOR
    public void InitializeForTests(
        int newDamagePerStack)
    {
        damagePerStack =
            Mathf.Max(
                0,
                newDamagePerStack);
    }
#endif

    private void OnValidate()
    {
        damagePerStack =
            Mathf.Max(
                0,
                damagePerStack);
    }
}