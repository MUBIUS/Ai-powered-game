using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject enemyPrefab;
    public Transform[] spawnPoints;

    private Player player;
    private QLearningAgent globalQLearningAgent;  // Global Q-learning agent shared between enemies

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        globalQLearningAgent = new QLearningAgent(0.1f, 0.9f);  // Initialize the global Q-learning agent
        globalQLearningAgent.Initialize();  // Initialize Q-table
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void OnEnemyKilled()
    {
        Debug.Log("Enemy killed!");
        Invoke("SpawnEnemy", 3f);  // Respawn the enemy after 3 seconds
    }

    // Spawns an enemy with access to the global Q-learning agent
    public void SpawnEnemy()
    {
        if (enemyPrefab != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();

            // Pass the global Q-learning agent to the enemy so it uses improved strategies
            enemyComponent.qLearningAgent = globalQLearningAgent;
        }
        else
        {
            Debug.LogError("Enemy prefab or spawn points not set up correctly!");
        }
    }
}
