using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject enemyPrefab;
    public List<Enemy> enemies = new List<Enemy>();

    public int waveNumber = 1;
    public int enemiesPerWave = 5;

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
        SpawnEnemies(enemiesPerWave);
    }

    // Player attack logic
    public void HandlePlayerAttack(Player player)
    {
        foreach (Enemy enemy in enemies)
        {
            if (Vector3.Distance(player.transform.position, enemy.transform.position) <= player.attackRange)
            {
                enemy.TakeDamage(player.attackDamage);
                break;
            }
        }
    }

    // Enemy attack logic
    public void HandleEnemyAttack(Enemy enemy)
    {
        Player player = FindObjectOfType<Player>();
        if (Vector3.Distance(enemy.transform.position, player.transform.position) <= enemy.attackRange)
        {
            player.TakeDamage(enemy.damage);
        }
    }

    // Spawn enemies at the start of each wave
    private void SpawnEnemies(int numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            Enemy enemyScript = newEnemy.GetComponent<Enemy>();
            enemies.Add(enemyScript);
        }
    }

    // Remove enemy from the game when dead
    public void RemoveEnemy(Enemy enemy)
    {
        enemies.Remove(enemy);

        // Check if all enemies are dead
        if (enemies.Count == 0)
        {
            waveNumber++;
            SpawnEnemies(enemiesPerWave + waveNumber);  // Increase the number of enemies per wave
        }
    }
}
