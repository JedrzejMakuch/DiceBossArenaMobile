using UnityEngine;

public abstract class SkillEffectDefinition : ScriptableObject
{
    public abstract bool CanApply(
        SkillExecutionContext context);

    public abstract void Apply(
        SkillExecutionContext context);
}