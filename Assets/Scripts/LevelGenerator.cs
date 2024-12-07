using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Fish Spawning")]
    public GameObject[] foodFishPrefabs;  // Array of different food fish prefabs
    public int fishPerLevel = 10;         // Number of fish to spawn per level
    public float minDistance = 2f;        // Minimum distance between fish
    
    [Header("Spawn Area")]
    public float spawnAreaWidth = 20f;    // Width of the spawn area
    public float spawnAreaHeight = 10f;   // Height of the spawn area
    public bool visualizeSpawnArea = true; // Draw spawn area in editor
    
    [Header("Difficulty Settings")]
    public int currentLevel = 1;
    public float difficultyMultiplier = 1.2f; // Increase fish count per level

    private void Start()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        // Calculate number of fish based on level
        int fishToSpawn = Mathf.RoundToInt(fishPerLevel * Mathf.Pow(difficultyMultiplier, currentLevel - 1));
        
        // Clear existing food fish
        GameObject[] existingFish = GameObject.FindGameObjectsWithTag("Food");
        foreach (GameObject fish in existingFish)
        {
            Destroy(fish);
        }

        // Spawn new fish
        for (int i = 0; i < fishToSpawn; i++)
        {
            SpawnFish(20); // Try 20 times to find a valid position
        }
    }

    private void SpawnFish(int maxAttempts)
    {
        Vector2 spawnPos = Vector2.zero;
        bool validPosition = false;
        int attempts = 0;

        // Try to find a valid position
        while (!validPosition && attempts < maxAttempts)
        {
            // Generate random position within spawn area
            spawnPos = new Vector2(
                Random.Range(-spawnAreaWidth/2, spawnAreaWidth/2),
                Random.Range(-spawnAreaHeight/2, spawnAreaHeight/2)
            );

            // Check if position is far enough from other fish
            validPosition = true;
            GameObject[] existingFish = GameObject.FindGameObjectsWithTag("Food");
            foreach (GameObject fish in existingFish)
            {
                if (Vector2.Distance(spawnPos, fish.transform.position) < minDistance)
                {
                    validPosition = false;
                    break;
                }
            }
            attempts++;
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
            
            // Set level bounds in the fish behavior
            fishBehavior.levelBounds = new Vector2(spawnAreaWidth, spawnAreaHeight);
            
            // Randomize speed slightly
            fishBehavior.swimSpeed = Random.Range(2f, 4f);
            fishBehavior.detectionRange = Random.Range(4f, 6f);
        }
    }

    private void OnDrawGizmos()
    {
        if (!visualizeSpawnArea) return;

        // Draw spawn area boundaries in editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(spawnAreaWidth, spawnAreaHeight, 0));
    }

    // Call this to generate next level
    public void NextLevel()
    {
        currentLevel++;
        GenerateLevel();
    }
}
