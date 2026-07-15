using System;
using System.Collections.Generic;
using DiceBossArena.Game;
using UnityEngine;

public sealed class FightPlayerBuildDevConfigurator :
    MonoBehaviour
{
    [Header("Fight References")]
    [SerializeField]
    private FightDeploymentManager deploymentManager;

    [Header("Character Build")]
    [SerializeField]
    private ClassDefinition classDefinition;

    [SerializeField]
    private SpecializationDefinition
        specializationDefinition;

    [SerializeField]
    private List<CharacterSkillDefinitionEntry>
        selectedSkills = new();

    private void Awake()
    {
        ConfigureBuild();
    }

    private void ConfigureBuild()
    {
        if (deploymentManager == null)
        {
            Debug.LogError(
                "FightPlayerBuildDevConfigurator requires " +
                "a FightDeploymentManager.",
                this);

            return;
        }

        if (classDefinition == null)
        {
            Debug.LogError(
                "FightPlayerBuildDevConfigurator requires " +
                "a ClassDefinition.",
                this);

            return;
        }

        try
        {
            List<CharacterBuildSkill> buildSkills =
                CreateSelectedSkills();

            CharacterBuildCompositionRequest request =
                new CharacterBuildCompositionRequest(
                    classDefinition,
                    specializationDefinition,
                    buildSkills);

            CharacterBuildSnapshot snapshot =
                new CharacterBuildComposer()
                    .Compose(
                        request);

            deploymentManager.ConfigurePlayerSpawnData(
                snapshot,
                FightUnitRuntimeSnapshot.Fresh);

            Debug.Log(
                $"Configured dev player build: " +
                $"{snapshot.ClassId.Value} / " +
                $"{snapshot.SpecializationId.Value}.",
                this);
        }
        catch (Exception exception)
        {
            Debug.LogError(
                $"Could not configure dev player build. " +
                $"{exception.Message}",
                this);
        }
    }

    private List<CharacterBuildSkill>
        CreateSelectedSkills()
    {
        List<CharacterBuildSkill> result =
            new();

        if (selectedSkills == null)
        {
            return result;
        }

        for (int i = 0;
             i < selectedSkills.Count;
             i++)
        {
            CharacterSkillDefinitionEntry entry =
                selectedSkills[i];

            if (entry == null ||
                !entry.IsValid)
            {
                throw new InvalidOperationException(
                    "Selected dev skill entry is invalid.");
            }

            result.Add(
                entry.CreateBuildSkill());
        }

        return result;
    }
}