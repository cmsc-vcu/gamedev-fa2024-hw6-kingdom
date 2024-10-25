using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Enemy prefab to spawn (assign this in the Inspector)
    public GameObject enemyPrefab;

    // References to the spawn points
    public Transform leftSpawnPoint;
    public Transform rightSpawnPoint;

    // Time interval between spawns
    public float spawnInterval = 3f;

    // Track the time since the last spawn
    private float timeSinceLastSpawn = 0f;

    void Update()
    {
        // Increase the timer
        timeSinceLastSpawn += Time.deltaTime;

        // Spawn enemy if timeSinceLastSpawn exceeds spawnInterval
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f; // Reset the timer
        }
    }

    // Function to spawn the enemy
    void SpawnEnemy()
    {
        if (enemyPrefab != null)
        {
            // Randomly choose a spawn point (left or right)
            Transform spawnPoint = Random.value < 0.5f ? leftSpawnPoint : rightSpawnPoint;

            // Instantiate the enemy prefab at the chosen spawn point's position and rotation
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Enemy prefab is not assigned!");
        }
    }
}
