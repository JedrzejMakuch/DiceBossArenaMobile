using System.Collections.Generic;

public sealed class StatusEffectBehaviorExecutor
{
    public int Execute(
        FightUnit owner,
        StatusEffectRuntimeState state)
    {
        StatusEffectExecutionContext context =
            new StatusEffectExecutionContext(
                owner,
                state);

        IReadOnlyList<
            StatusEffectBehaviorDefinition> behaviors =
                state.Definition.Behaviors;

        if (behaviors == null ||
            behaviors.Count == 0)
        {
            return 0;
        }

        int executedCount = 0;

        for (int i = 0; i < behaviors.Count; i++)
        {
            StatusEffectBehaviorDefinition behavior =
                behaviors[i];

            if (behavior == null ||
                !behavior.CanExecute(context))
            {
                continue;
            }

            behavior.Execute(
                context);

            executedCount++;

            if (!owner.IsAlive)
            {
                break;
            }
        }

        return executedCount;
    }
}