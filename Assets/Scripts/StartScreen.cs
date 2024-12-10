using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StartScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button startButton;
    [SerializeField] private Image titleImage;
    [SerializeField] private CanvasGroup startScreenCanvas;

    [Header("Game References")]
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private GameObject player;

    [Header("Game UI References")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Slider hungerMeter;
    [SerializeField] private FixedJoystick joystick;
    [SerializeField] private Button restartButton;

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

        // Hide game UI elements initially
        if (gameOverText != null) gameOverText.gameObject.SetActive(false);
        if (hungerMeter != null) hungerMeter.gameObject.SetActive(false);
        if (joystick != null) joystick.gameObject.SetActive(false);
        if (restartButton != null) restartButton.gameObject.SetActive(false);

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
            startButton.gameObject.SetActive(false);
            titleImage.gameObject.SetActive(false);
        }

        // Enable player and start the game
        if (player != null)
        {
            player.SetActive(true);
        }

        // Show game UI elements
        if (hungerMeter != null) hungerMeter.gameObject.SetActive(true);
        if (joystick != null) joystick.gameObject.SetActive(true);
        // Note: gameOverText stays hidden until needed by the Player script

        // Generate the first level
        if (levelGenerator != null)
        {
            levelGenerator.GenerateLevel();
        }
    }
}
