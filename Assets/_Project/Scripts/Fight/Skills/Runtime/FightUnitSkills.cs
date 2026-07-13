using System;
using System.Collections.Generic;
using UnityEngine;

public class FightUnitSkills : MonoBehaviour
{
    [Header("Assigned Skills")]
    [SerializeField] private List<UnitSkillState> skills = new();

    public IReadOnlyList<UnitSkillState> Skills => skills;
    public event Action<FightUnitSkills> SkillsChanged;

    private void Awake()
    {
        ValidateSkills();
    }

    public void InitializeFromDefinition(
    IReadOnlyList<UnitStartingSkill> startingSkills)
    {
        skills = new List<UnitSkillState>();

        if (startingSkills != null)
        {
            foreach (UnitStartingSkill startingSkill in startingSkills)
            {
                if (startingSkill?.Definition == null)
                {
                    continue;
                }

                if (GetSkillState(startingSkill.Definition) != null)
                {
                    continue;
                }

                skills.Add(
                    new UnitSkillState(
                        startingSkill.Definition,
                        startingSkill.Level));
            }
        }

        SkillsChanged?.Invoke(this);
    }

    public UnitSkillState GetSkillById(
        string skillId)
    {
        if (string.IsNullOrWhiteSpace(skillId))
        {
            return null;
        }

        foreach (UnitSkillState skill in skills)
        {
            if (skill?.Definition == null)
            {
                continue;
            }

            if (skill.Definition.SkillId == skillId)
            {
                return skill;
            }
        }

        return null;
    }

    public bool HasSkill(
        SkillDefinition definition)
    {
        return GetSkillState(definition) != null;
    }

    public UnitSkillState GetSkillState(
        SkillDefinition definition)
    {
        if (definition == null)
        {
            return null;
        }

        foreach (UnitSkillState skill in skills)
        {
            if (skill?.Definition == definition)
            {
                return skill;
            }
        }

        return null;
    }

    public bool AddSkill(
        SkillDefinition definition,
        int level = 1)
    {
        if (definition == null)
        {
            return false;
        }

        UnitSkillState existingSkill =
            GetSkillState(definition);

        if (existingSkill != null)
        {
            Debug.LogWarning(
                $"{name} already has skill " +
                $"{definition.DisplayName}.",
                this);

            return false;
        }

        skills.Add(
            new UnitSkillState(
                definition,
                level));

        SkillsChanged?.Invoke(this);

        Debug.Log(
            $"{name} learned skill " +
            $"{definition.DisplayName} at level {level}.",
            this);

        return true;
    }

    public bool RemoveSkill(
        SkillDefinition definition)
    {
        UnitSkillState skill =
            GetSkillState(definition);

        if (skill == null)
        {
            return false;
        }

        bool removed = skills.Remove(skill);

        if (!removed)
        {
            return false;
        }

        SkillsChanged?.Invoke(this);

        Debug.Log(
            $"{name} removed skill " +
            $"{definition.DisplayName}.",
            this);

        return true;
    }

    public void ReduceCooldowns()
    {
        bool anyChanged = false;

        foreach (UnitSkillState skill in skills)
        {
            if (skill == null ||
                skill.CurrentCooldown <= 0)
            {
                continue;
            }

            skill.ReduceCooldown();
            anyChanged = true;
        }

        if (anyChanged)
        {
            SkillsChanged?.Invoke(this);
        }
    }

    public void ResetAllCooldowns()
    {
        foreach (UnitSkillState skill in skills)
        {
            skill?.ResetCooldown();
        }

        SkillsChanged?.Invoke(this);
    }

    private void ValidateSkills()
    {
        if (skills == null)
        {
            skills = new List<UnitSkillState>();
            return;
        }

        HashSet<SkillDefinition> definitions = new();

        for (int i = skills.Count - 1; i >= 0; i--)
        {
            UnitSkillState skill = skills[i];

            if (skill == null ||
                skill.Definition == null)
            {
                skills.RemoveAt(i);
                continue;
            }

            if (!definitions.Add(skill.Definition))
            {
                Debug.LogWarning(
                    $"{name} contains duplicate skill " +
                    $"{skill.Definition.DisplayName}. " +
                    $"Duplicate entry removed.",
                    this);

                skills.RemoveAt(i);
            }
        }
    }
}