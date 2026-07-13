using System;
using UnityEngine;

[Serializable]
public sealed class UnitStartingSkill
{
    [SerializeField] private SkillDefinition definition;
    [SerializeField, Min(1)] private int level = 1;

    public SkillDefinition Definition => definition;
    public int Level => level;

    public UnitStartingSkill(
        SkillDefinition definition,
        int level)
    {
        this.definition = definition;
        this.level = Mathf.Max(1, level);
    }
}