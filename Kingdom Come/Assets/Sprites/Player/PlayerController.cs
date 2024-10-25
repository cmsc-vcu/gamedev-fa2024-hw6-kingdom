using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float attackCooldown = 2f;  // Cooldown time in seconds
    private Animator animator;
    private Rigidbody2D rb;

    private bool isHurt = false;  // To prevent movement when hurt
    private bool canAttack = true;  // Track if player can attack
    private float nextAttackTime = 0f;  // Track when next attack is allowed

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isHurt)
        {
            return;  // Don't allow movement or attack while hurt
        }

        // Get input for movement
        float moveInput = Input.GetAxisRaw("Horizontal");

        // Move the player
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);

        // Check if the player is moving
        if (Mathf.Abs(moveInput) > 0)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }

        // Flip the player if moving left
        if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Handle attack with cooldown
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextAttackTime)  // Example: Space key for attack
        {
            Attack();
        }
    }

    private void Attack()
    {
        animator.SetTrigger("attack");
        nextAttackTime = Time.time + attackCooldown;  // Set the time for the next attack
    }

    // Call this method when the player is hurt
    public void Hurt()
    {
        isHurt = true;
        animator.SetTrigger("hurt");
        rb.velocity = Vector2.zero;  // Stop movement when hurt
    }

    // Method to reset hurt state after the hurt animation completes (use Animation Event)
    public void EndHurt()
    {
        isHurt = false;
    }
}
