using UnityEngine;

public class FightSceneInitializer : MonoBehaviour
{
    [Header("Fight Scene Systems")]
    [SerializeField] private FightArenaGenerator arenaGenerator;
    [SerializeField] private FightDeploymentManager deploymentManager;
    [SerializeField] private EnemySpawnManager enemySpawnManager;

    private void Start()
    {
        InitializeFightScene();
    }

    private void InitializeFightScene()
    {
        if (!ValidateReferences())
        {
            return;
        }

        Debug.Log("FightScene initialization started.");

        arenaGenerator.GenerateArena();
        deploymentManager.PrepareDeployment();
        enemySpawnManager.SpawnEnemies();

        Debug.Log("FightScene initialization completed.");
    }

    private bool ValidateReferences()
    {
        bool isValid = true;

        if (arenaGenerator == null)
        {
            Debug.LogError(
                "FightSceneInitializer: Arena Generator is not assigned.",
                this);

            isValid = false;
        }

        if (deploymentManager == null)
        {
            Debug.LogError(
                "FightSceneInitializer: Deployment Manager is not assigned.",
                this);

            isValid = false;
        }

        if (enemySpawnManager == null)
        {
            Debug.LogError(
                "FightSceneInitializer: Enemy Spawn Manager is not assigned.",
                this);

            isValid = false;
        }

        return isValid;
    }
}