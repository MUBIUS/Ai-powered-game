using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int initialEnemyCount = 3;
    public float respawnDelay = 3f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        SpawnPlayer();
        for (int i = 0; i < initialEnemyCount; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            GameObject playerObj = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Player player = playerObj.GetComponent<Player>();
            if (player == null)
            {
                Debug.LogError("Player component not found on the instantiated player prefab!");
            }
        }
        else
        {
            Debug.LogError("Player prefab not set in GameManager!");
        }
    }

    public void OnEnemyKilled()
    {
        Debug.Log("Enemy killed!");
        Invoke("SpawnEnemy", respawnDelay);
    }

    public void SpawnEnemy()
    {
        if (enemyPrefab != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            Enemy enemyComponent = newEnemy.GetComponent<Enemy>();

            if (enemyComponent == null)
            {
                Debug.LogError("Enemy component not found on the instantiated enemy prefab!");
            }
        }
        else
        {
            Debug.LogError("Enemy prefab or spawn points not set up correctly!");
        }
    }
}