using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Fish Spawning")]
    public GameObject[] foodFishPrefabs;  // Array of different food fish prefabs
    public int fishPerLevel = 10;         // Number of fish to spawn per level
    public float minDistance = 2f;        // Minimum distance between fish
    
    [Header("Spawn Area")]
    public float playAreaWidth = 20f;     // Width of the play area
    public float playAreaHeight = 10f;    // Height of the play area
    public float spawnMargin = 5f;        // Extra margin outside play area for spawning
    public bool visualizeSpawnArea = true; // Draw spawn area in editor

    [Header("Difficulty Settings")]
    public int currentLevel = 1;
    public int maxLevel = 10;  // Maximum level
    public float difficultyMultiplier = 1.2f; // Increase fish count per level
    public int minFishPerLevel = 5;  // Minimum fish to spawn
    public int maxFishPerLevel = 20; // Maximum fish to spawn

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        Debug.Log($"Generating Level {currentLevel}");
        // Calculate number of fish based on level, with min and max limits
        float multiplier = Mathf.Pow(difficultyMultiplier, currentLevel - 1);
        int fishToSpawn = Mathf.RoundToInt(fishPerLevel * multiplier);
        fishToSpawn = Mathf.Clamp(fishToSpawn, minFishPerLevel, maxFishPerLevel);
        
        Debug.Log($"Attempting to spawn {fishToSpawn} fish");
        // Clear existing food fish
        GameObject[] existingFish = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject fish in existingFish)
        {
            Destroy(fish);
        }

        // Calculate number of fish based on level
        int fishToSpawn = Mathf.RoundToInt(fishPerLevel * Mathf.Pow(difficultyMultiplier, currentLevel - 1));
        Debug.Log(fishToSpawn + " fishes to spawn");

        // Spawn new fish
        for (int i = 0; i < fishToSpawn; i++)
        {
            SpawnFish();
        }
    }

    private void SpawnFish()
    {
        Vector2 spawnPos = Vector2.zero;
        bool validPosition = false;
        GameObject[] existingFish = GameObject.FindGameObjectsWithTag("Food");

        // Try to find a valid position
        while (!validPosition)
        {
            // Randomly choose which side to spawn from (0: top, 1: right, 2: bottom, 3: left)
            int side = Random.Range(0, 4);
            float totalWidth = playAreaWidth + (spawnMargin * 2);
            float totalHeight = playAreaHeight + (spawnMargin * 2);

            switch (side)
            {
                case 0: // Top
                    spawnPos = new Vector2(
                        Random.Range(-totalWidth/2, totalWidth/2),
                        totalHeight/2
                    );
                    break;
                case 1: // Right
                    spawnPos = new Vector2(
                        totalWidth/2,
                        Random.Range(-totalHeight/2, totalHeight/2)
                    );
                    break;
                case 2: // Bottom
                    spawnPos = new Vector2(
                        Random.Range(-totalWidth/2, totalWidth/2),
                        -totalHeight/2
                    );
                    break;
                case 3: // Left
                    spawnPos = new Vector2(
                        -totalWidth/2,
                        Random.Range(-totalHeight/2, totalHeight/2)
                    );
                    break;
            }

            // Check if position is far enough from other fish
            foreach (GameObject fish in existingFish)
            {
                if (Vector2.Distance(spawnPos, fish.transform.position) < minDistance)
                {
                    validPosition = false;
                    break;
                }
                else
                {
                    validPosition = true;
                }
            }
        }

        if (validPosition)
        {
            // Select random fish prefab
            GameObject fishPrefab = foodFishPrefabs[Random.Range(0, foodFishPrefabs.Length)];
            
            // Spawn the fish
            GameObject newFish = Instantiate(fishPrefab, spawnPos, Quaternion.identity);
            newFish.tag = "Food";

            // Add FoodFish behavior component
            FoodFish fishBehavior = newFish.AddComponent<FoodFish>();
            
            // Set bounds for the fish behavior (actual play area, not spawn area)
            fishBehavior.levelBounds = new Vector2(playAreaWidth, playAreaHeight);
            
            // Randomize speed and detection range
            fishBehavior.swimSpeed = Random.Range(2f, 4f);
            fishBehavior.detectionRange = Random.Range(4f, 6f);

            // Set initial target position within play area
            float targetX = Random.Range(-playAreaWidth/2, playAreaWidth/2);
            float targetY = Random.Range(-playAreaHeight/2, playAreaHeight/2);
            fishBehavior.SetInitialTarget(new Vector2(targetX, targetY));
        }
    }

    private void OnDrawGizmos()
    {
        if (!visualizeSpawnArea) return;

        // Draw play area boundaries
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(playAreaWidth, playAreaHeight, 0));

        // Draw spawn area boundaries
        Gizmos.color = Color.yellow;
        float totalWidth = playAreaWidth + (spawnMargin * 2);
        float totalHeight = playAreaHeight + (spawnMargin * 2);
        Gizmos.DrawWireCube(transform.position, new Vector3(totalWidth, totalHeight, 0));
    }

    // Call this to generate next level
    public void NextLevel()
    {
        Debug.Log($"NextLevel called. Current level: {currentLevel}, Max level: {maxLevel}");
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            Debug.Log($"Moving to level {currentLevel}");
            GenerateLevel();
        }
        else
        {
            Debug.Log("Max level reached!");
            // Still generate fish even at max level
            GenerateLevel();
        }
    }
}
