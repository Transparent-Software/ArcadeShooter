using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform playerTransform;
    public float spawnDelay = 2f;
    public float spawnRadius = 10f;
    public float movementSpeed = 5f;

    private void Start()
    {
        InvokeRepeating("SpawnEnemy", spawnDelay, spawnDelay);
    }

    private void SpawnEnemy()
    {
        Vector3 randomPosition = GetRandomPositionAroundPlayer();
        GameObject enemy = Instantiate(enemyPrefab, randomPosition, Quaternion.identity);
        EnemyMovement enemyMovement = enemy.GetComponent<EnemyMovement>();

        if (enemyMovement == null)
        {
            Debug.LogError("EnemyMovement component not found on the enemy prefab.");
            return;
        }

        if (playerTransform == null)
        {
            Debug.LogError("Player transform not assigned to the EnemySpawner.");
            return;
        }

        enemyMovement.SetTarget(playerTransform);
        enemyMovement.movementSpeed = movementSpeed; // Pass the movement speed to the enemy
    }

    private Vector3 GetRandomPositionAroundPlayer()
    {
        Vector2 randomCirclePoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 randomPosition = playerTransform.position + new Vector3(randomCirclePoint.x, 0f, randomCirclePoint.y);
        return randomPosition;
    }
}
