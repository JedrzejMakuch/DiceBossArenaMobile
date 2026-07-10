using UnityEngine;
using UnityEngine.SceneManagement;

public class DevSceneNavigator : MonoBehaviour
{
    [SerializeField] private string boardSceneName = "Prototype_GeneratedBoard_DEV_PANEL";
    [SerializeField] private string fightSceneName = "Prototype_FightScene";

    public void LoadTestFight()
    {
        SceneManager.LoadScene(fightSceneName, LoadSceneMode.Single);
    }

    public void LoadBoard()
    {
        SceneManager.LoadScene(boardSceneName, LoadSceneMode.Single);
    }

    public void RestartCurrentScene()
    {
        var currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name, LoadSceneMode.Single);
    }
}