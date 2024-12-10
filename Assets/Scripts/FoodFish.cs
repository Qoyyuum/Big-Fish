using UnityEngine;

public class FoodFish : MonoBehaviour
{
    [HideInInspector] public Vector2 levelBounds = new Vector2(20f, 10f); 
    [HideInInspector] public float swimSpeed = 3f;
    [HideInInspector] public float detectionRange = 5f;

    private Transform player;
    private Vector2 wanderTarget;
    private float nextDirectionChange;
    private float directionChangeInterval = 3f;
    private float minDistanceFromBounds = 1f;
    private SpriteRenderer spriteRenderer;
    private Vector2 initialTarget;
    private bool isSwimmingToInitialTarget = true;
    private Rigidbody2D rb;

    void Start()
    {
        // Find the player
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Set initial wander target to current position
        wanderTarget = transform.position;
        
        // Add a small random variation to speed
        swimSpeed *= Random.Range(0.8f, 1.2f);
    }

    public void SetInitialTarget(Vector2 target)
    {
        initialTarget = target;
        isSwimmingToInitialTarget = true;
    }

    void Update()
    {
        if (player == null) return;

        Vector2 moveDirection;
        float currentSpeed = swimSpeed;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isSwimmingToInitialTarget)
        {
            // Swim towards initial target
            moveDirection = initialTarget - (Vector2)transform.position;
            if (moveDirection.magnitude < 0.1f)
            {
                isSwimmingToInitialTarget = false;
                SetNewWanderTarget();
            }
        }
        else if (distanceToPlayer < detectionRange)
        {
            // If player is within detection range, swim away
            moveDirection = (Vector2)transform.position - (Vector2)player.position;
            currentSpeed *= 1.5f; // Swim faster when escaping
        }
        else
        {
            // Normal wandering behavior
            if (Time.time > nextDirectionChange)
            {
                SetNewWanderTarget();
            }
            moveDirection = wanderTarget - (Vector2)transform.position;
        }

        // Normalize direction and apply speed
        moveDirection.Normalize();
        Vector2 movement = moveDirection * currentSpeed * Time.deltaTime;

        // Check bounds and adjust movement if necessary
        Vector2 newPosition = (Vector2)transform.position + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, -levelBounds.x/2 + minDistanceFromBounds, levelBounds.x/2 - minDistanceFromBounds);
        newPosition.y = Mathf.Clamp(newPosition.y, -levelBounds.y/2 + minDistanceFromBounds, levelBounds.y/2 - minDistanceFromBounds);

        // Move the fish
        transform.position = newPosition;

        // Flip sprite based on movement direction
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    void SetNewWanderTarget()
    {
        // Set a random point within wander radius
        float wanderRadius = 3f;
        float randomX = Random.Range(-wanderRadius, wanderRadius);
        float randomY = Random.Range(-wanderRadius, wanderRadius);
        wanderTarget = (Vector2)transform.position + new Vector2(randomX, randomY);

        // Clamp to level bounds
        wanderTarget.x = Mathf.Clamp(wanderTarget.x, -levelBounds.x/2 + minDistanceFromBounds, levelBounds.x/2 - minDistanceFromBounds);
        wanderTarget.y = Mathf.Clamp(wanderTarget.y, -levelBounds.y/2 + minDistanceFromBounds, levelBounds.y/2 - minDistanceFromBounds);

        // Set time for next direction change
        nextDirectionChange = Time.time + directionChangeInterval * Random.Range(0.8f, 1.2f);
    }

    // Optional: Visualize detection range in editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
