using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private Button rollButton;
    [SerializeField] private GameObject restartButton;


    private bool isGameOver;

    public bool IsGameOver => isGameOver;

    private void Start()
    {
        if (gameOverText != null)
            gameOverText.SetActive(false);
    }

    public void TriggerGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameOverText != null)
            gameOverText.SetActive(true);

        if (restartButton != null)
            restartButton.SetActive(true);

        if (rollButton != null)
            rollButton.interactable = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}