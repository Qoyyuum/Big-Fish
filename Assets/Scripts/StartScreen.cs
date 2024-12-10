using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private CanvasGroup startScreenCanvas;

    [Header("Game References")]
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private GameObject player;

    private void Start()
    {
        // Ensure the start screen is visible and the game is paused
        if (startScreenCanvas != null)
        {
            startScreenCanvas.alpha = 1;
            startScreenCanvas.interactable = true;
            startScreenCanvas.blocksRaycasts = true;
        }

        // Disable player movement initially
        if (player != null)
        {
            player.SetActive(false);
        }

        // Add listener to start button
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartGame);
        }
    }

    public void StartGame()
    {
        // Hide the start screen
        if (startScreenCanvas != null)
        {
            startScreenCanvas.alpha = 0;
            startScreenCanvas.interactable = false;
            startScreenCanvas.blocksRaycasts = false;
        }

        // Enable player and start the game
        if (player != null)
        {
            player.SetActive(true);
        }

        // Generate the first level
        if (levelGenerator != null)
        {
            levelGenerator.GenerateLevel();
        }
    }
}
