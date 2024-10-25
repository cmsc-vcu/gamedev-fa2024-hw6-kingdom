using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Needed for UI elements
using UnityEngine.SceneManagement; // Needed to manage scenes

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float attackCooldown = 2f; // Cooldown time in seconds
    public int maxHealth = 100; // Max health of the player
    public GameObject gameOverPanel; // Panel to display game over UI
    public Button restartButton; // Button to restart the game
    public float attackRange = 1f; // Range for attack detection
    public LayerMask enemyLayer; // LayerMask to identify enemy objects
    public int attackDamage = 10; // Damage dealt to enemies

    private Animator animator;
    private Rigidbody2D rb;
    private int currentHealth; // Current health of the player
    private bool isHurt = false; // To prevent movement when hurt
    private float nextAttackTime = 0f; // Track when next attack is allowed

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth; // Set the player's health to max at start
        gameOverPanel.SetActive(false); // Hide the game over panel at start

        // Assign restart button click event
        restartButton.onClick.AddListener(RestartGame);
    }

    void Update()
    {
        if (isHurt || currentHealth <= 0)
        {
            return; // Don't allow movement or attack while hurt or dead
        }

        // Get input for movement
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Move the player
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Check if the player is moving
        animator.SetBool("isRunning", Mathf.Abs(moveInput) > 0);

        // Flip the player if moving left or right
        transform.localScale = new Vector3(moveInput < 0 ? -1 : 1, 1, 1);

        // Handle attack with cooldown
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime) // Example: Space key for attack
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("attack"); // Trigger the attack animation
        nextAttackTime = Time.time + attackCooldown; // Set the time for the next attack

        // Check for enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().GotHit(attackDamage); // Call the GotHit method on the enemy
            Debug.Log($"Attacked enemy for {attackDamage} damage."); // Optional debug message
        }
    }

    // Call this method when the player is hurt
    public void Hurt(int damage)
    {
        currentHealth -= damage; // Reduce current health by damage amount

        if (currentHealth <= 0)
        {
            Die();
            return; // If the player is dead, exit the method
        }

        isHurt = true;
        animator.SetTrigger("hurt");
        rb.velocity = Vector2.zero; // Stop movement when hurt
    }

    private void Die()
    {
        animator.SetTrigger("death"); // Play death animation
        gameOverPanel.SetActive(true); // Show the game over UI
        rb.velocity = Vector2.zero; // Stop all movement
        Time.timeScale = 0; // Stop the game (optional)
    }

    // Method to reset hurt state after the hurt animation completes (use Animation Event)
    public void EndHurt()
    {
        isHurt = false;
    }

    // Restart the game
    private void RestartGame()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload the current scene
    }

    // Visualize attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange); // Show attack range in the scene view
    }
}
