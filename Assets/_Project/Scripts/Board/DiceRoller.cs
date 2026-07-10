using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerBoardMover playerMover;
    [SerializeField] private Button rollButton;
    [SerializeField] private TMP_Text diceResultText;

    [Header("Dice")]
    [SerializeField] private int minRoll = 1;
    [SerializeField] private int maxRoll = 6;

    private void Awake()
    {
        if (rollButton != null)
        {
            rollButton.onClick.AddListener(RollDice);
        }
    }

    private void OnEnable()
    {
        if (playerMover != null)
        {
            playerMover.OnMovementFinished += HandleMovementFinished;
        }
    }

    private void OnDisable()
    {
        if (playerMover != null)
        {
            playerMover.OnMovementFinished -= HandleMovementFinished;
        }

        if (rollButton != null)
        {
            rollButton.onClick.RemoveListener(RollDice);
        }
    }

    public void RollDice()
    {
        if (playerMover == null)
        {
            Debug.LogWarning("DiceRoller: Player mover is missing.");
            return;
        }

        if (playerMover.IsMoving)
            return;

        int diceRoll = Random.Range(minRoll, maxRoll + 1);

        Debug.Log("Dice roll: " + diceRoll);

        if (diceResultText != null)
            diceResultText.text = "Roll: " + diceRoll;

        SetRollButtonInteractable(false);

        playerMover.MoveBySteps(diceRoll);
    }

    private void HandleMovementFinished(BoardTile landedTile)
    {
        SetRollButtonInteractable(true);
    }

    private void SetRollButtonInteractable(bool isInteractable)
    {
        if (rollButton != null)
        {
            rollButton.interactable = isInteractable;
        }
    }
}