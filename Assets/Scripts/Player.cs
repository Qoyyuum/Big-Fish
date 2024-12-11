using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.UI; // Add this for UI components

public class Player : MonoBehaviour
{
    public FixedJoystick joystick;
    public float moveSpeed;

    float horizontalInput, verticalInput;
    int score = 0;

    [Header("Game UI")]
    public GameObject winText;
    public int winScore;
    public LevelGenerator levelGenerator;
    public Slider hungerMeter; // Reference to UI Slider
    public GameObject gameOverText; // Reference to Game Over text
    public GameObject restartButton; // Add reference to restart button

    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float currentHunger;
    public float hungerDecayRate = 5f; // Hunger points lost per second
    public float hungerGainOnEat = 30f; // Hunger points gained when eating a fish

    public AudioSource chompingSound;
    private bool isGameOver = false;

    void Start()
    {
        score = 0;
        winScore = 7;
        chompingSound = GetComponent<AudioSource>();
        currentHunger = maxHunger;
        if (hungerMeter != null)
        {
            hungerMeter.maxValue = maxHunger;
            hungerMeter.value = currentHunger;
        }
        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver)
            return;

        // Decrease hunger over time
        currentHunger -= hungerDecayRate * Time.deltaTime;
        
        // Update hunger meter UI
        if (hungerMeter != null)
        {
            hungerMeter.value = currentHunger;
        }

        // Check for game over
        if (currentHunger <= 0)
        {
            GameOver();
        }
    }

    private void FixedUpdate()
    {
        if (isGameOver)
            return;

        horizontalInput = joystick.Horizontal * moveSpeed;
        verticalInput = joystick.Vertical * moveSpeed;

        transform.Translate(horizontalInput, verticalInput, 0);
        if(horizontalInput < 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if(horizontalInput > 0)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag.Contains ("Food"))
        {
            Destroy(collision.gameObject);
            chompingSound.Play();

            // Gain hunger when eating
            currentHunger = Mathf.Min(currentHunger + hungerGainOnEat, maxHunger);
            score++;
            Debug.Log("Your score is " + score);
            if (score >= winScore)
            {
                score = 0;
                Debug.Log("Your score now has reset to " + score);
                winText.SetActive(true);
                Invoke("StartNextLevel", 2f);
            }
        }
    }

    private void StartNextLevel()
    {
        score = 0;
        winScore += 1;
        winText.SetActive(false);
        // Restore some hunger when starting new level
        currentHunger = Mathf.Max(currentHunger, maxHunger * 0.5f);
        levelGenerator.NextLevel();
        Debug.Log("StartNextLevel");
    }

    private void GameOver()
    {
        isGameOver = true;
        if (gameOverText != null)
        {
            gameOverText.SetActive(true);
        }
        if (restartButton != null)
        {
            restartButton.SetActive(true);
        }
        // Optional: Add restart functionality here
    }

    // Call this method to restart the game
    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        currentHunger = maxHunger;
        if (gameOverText != null)
        {
            gameOverText.SetActive(false);
        }
        if (restartButton != null)
        {
            restartButton.SetActive(false);
        }
        if (winText != null)
        {
            winText.SetActive(false);
        }
        levelGenerator.currentLevel = 1;
        levelGenerator.GenerateLevel();
        Debug.Log("restart game");
    }
}
