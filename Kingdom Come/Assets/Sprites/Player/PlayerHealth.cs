using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100; // Maximum health of the player
    private int currentHealth; // Current health of the player

    public GameObject gameOverPanel; // Panel to display game over UI
    private Animator animator; // Animator for the player

    void Start()
    {
        currentHealth = maxHealth; // Set current health to max at the start
        animator = GetComponent<Animator>(); // Get the Animator component
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount; // Reduce current health by damage amount
        Debug.Log("Player took damage: " + amount + ". Current health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die(); // Call the die method if health reaches zero
        }
    }

    private void Die()
    {
        gameOverPanel.SetActive(true); // Show the game over UI
        Time.timeScale = 0; // Stop the game
        Debug.Log("Player has died.");
    }
}
