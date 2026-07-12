using System;
using UnityEngine;

[Serializable]
public class UnitSkillState
{
    [SerializeField] private SkillDefinition definition;

    [SerializeField, Min(1)] private int level = 1;

    [SerializeField, Min(0)] private int currentCooldown;

    public SkillDefinition Definition => definition;

    public int Level => level;

    public int CurrentCooldown => currentCooldown;

    public bool IsReady => definition != null && currentCooldown <= 0;

    public SkillLevelData LevelData => definition != null ? definition.GetLevelData(level) : SkillLevelData.Default;

    public UnitSkillState()
    { }

    public UnitSkillState(SkillDefinition definition, int level)
    {
        this.definition = definition;
        SetLevel(level);
        currentCooldown = 0;
    }

    public void SetLevel(int newLevel)
    {
        if (definition == null)
        {
            level = Mathf.Max(1, newLevel);
            return;
        }

        level = Mathf.Clamp(
            newLevel,
            1,
            definition.MaxLevel);
    }

    public bool TryStartCooldown()
    {
        if (definition == null)
        {
            return false;
        }

        int cooldown = LevelData.Cooldown;

        if (cooldown <= 0)
        {
            currentCooldown = 0;
            return true;
        }

        currentCooldown = cooldown;
        return true;
    }

    public void ReduceCooldown(int amount = 1)
    {
        if (amount <= 0 ||
            currentCooldown <= 0)
        {
            return;
        }

        currentCooldown = Mathf.Max(
            0,
            currentCooldown - amount);
    }

    public void ResetCooldown()
    {
        currentCooldown = 0;
    }
}