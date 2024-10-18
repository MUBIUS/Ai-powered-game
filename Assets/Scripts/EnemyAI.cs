using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour, IDamageable
{
    public int health = 50;
    public int attackDamage = 10;
    public float attackRange = 2f;
    public float chaseRange = 10f;  // Enemy starts chasing the player when within this range
    public float attackRate = 1f;
    public GameObject weaponDropPrefab;

    public bool shouldRespawn = true; // Set this to false if you don't want the enemy to respawn
    public float respawnTime = 5f; // Delay before the enemy respawns
    public Vector3 spawnPosition; // To store the original spawn position


    private float nextAttackTime = 0f;
    private Player player;
    private NavMeshAgent agent;
    private QLearningAgent qLearningAgent;

    private void Start()
    {
        player = FindObjectOfType<Player>();
        agent = GetComponent<NavMeshAgent>();
        qLearningAgent = new QLearningAgent(0.1f, 0.9f, 3); // Learning rate: 0.1, Discount factor: 0.9, Actions: 3

        if (player == null)
        {
            Debug.LogError("Player not found in the scene!");
        }

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on the Enemy!");
        }
    }

    private void Update()
    {
        if (player == null) return; // Exit if no player is found

        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        // If player is within chase range, move towards the player
        if (distanceToPlayer <= chaseRange)
        {
            agent.SetDestination(player.transform.position);
        }

        // If the enemy is within attack range and the cooldown has passed, attack the player
        if (distanceToPlayer <= attackRange && Time.time >= nextAttackTime)
        {
            AttackPlayer();
        }
    }

    private void AttackPlayer()
    {
        nextAttackTime = Time.time + 1f / attackRate;  // Attack cooldown
        Debug.Log("Enemy attacking the player!");

        if (player != null)
        {
            player.TakeDamage(attackDamage);  // Player takes damage
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Enemy took " + damage + " damage! Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        DropWeapon();

        if (shouldRespawn)
        {
            // Start respawn process
            StartCoroutine(RespawnEnemy());
        }
        else
        {
            Destroy(gameObject); // Destroy the enemy permanently if no respawn is required
        }
    }

    // Coroutine to respawn the enemy after a delay
    private IEnumerator RespawnEnemy()
    {
        // Hide the enemy temporarily
        gameObject.SetActive(false);

        // Wait for the respawn delay
        yield return new WaitForSeconds(respawnTime);

        // Reset health and position
        health = 50; // Reset health to full
        transform.position = spawnPosition; // Reset position to the original spawn point

        // Reactivate the enemy
        gameObject.SetActive(true);
    }

    private void DropWeapon()
    {
        if (weaponDropPrefab != null)
        {
            Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize chase range and attack range in the editor
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
