using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f; // Speed at which the enemy moves
    public int damage = 10; // Damage dealt to the player
    public float attackRange = 1.5f; // Range within which the enemy can attack
    public int health = 50; // Health of the enemy

    private GameObject player; // Reference to the player
    private Animator animator;
    private float attackCooldown = 1f; // Cooldown time between attacks
    private float nextAttackTime = 0f; // Track when next attack is allowed

    private void Start()
    {
        player = GameObject.FindWithTag("Player"); // Find the player by tag
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    private void Update()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance < attackRange && Time.time >= nextAttackTime)
            {
                AttackPlayer(); // Attack if within range
            }
            else if (distance < 5f) // Move towards the player if within a certain distance
            {
                MoveTowardsPlayer();
                animator.SetBool("isRunning", true); // Set running parameter to true
            }
            else
            {
                animator.SetBool("isRunning", false); // Set running parameter to false
            }
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        transform.position += (Vector3)direction * speed * Time.deltaTime; // Move towards player
    }

    private void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            animator.SetTrigger("attack"); // Trigger the attack animation
            nextAttackTime = Time.time + attackCooldown; // Set the cooldown for the next attack
        }
        else
        {
            Debug.LogError("PlayerHealth component not found on player object.");
        }
    }

    // This method is called by an Animation Event
    public void DealDamageToPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage); // Deal damage to the player
            Debug.Log("Attacked player for: " + damage + " damage.");
        }
    }

    // Call this method when the enemy gets hit
    public void GotHit(int damage)
    {
        health -= damage; // Reduce enemy health by damage amount
        animator.SetTrigger("gotHit"); // Trigger the got hit animation
        
        // Check if the enemy is dead
        if (health <= 0)
        {
            Die(); // Call the Die method if health is zero or less
        }
    }

    // Call this method when the enemy dies
    public void Die()
    {
        animator.SetTrigger("death"); // Trigger the death animation
        // Optionally disable the enemy or destroy it after a delay
        Destroy(gameObject, 1f); // Delay before destroying the enemy object
    }
}
