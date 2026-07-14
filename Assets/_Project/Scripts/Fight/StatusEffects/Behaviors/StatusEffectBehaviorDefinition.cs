using UnityEngine;

public abstract class StatusEffectBehaviorDefinition :
    ScriptableObject
{
    public virtual bool CanExecute(
        StatusEffectExecutionContext context)
    {
        return context != null &&
               context.Owner != null &&
               context.Owner.IsAlive &&
               context.State != null &&
               !context.State.IsExpired;
    }

    public abstract void Execute(
        StatusEffectExecutionContext context);
}