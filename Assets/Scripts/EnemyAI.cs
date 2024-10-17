using UnityEditor.Build.Content;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameManager gameManager;
    
    public int health = 50;
    public float speed = 3f;
    public int damage = 10;
    public float attackRange = 2f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;  // Find player
    }

    private void Update()
    {
        HandleMovement();

        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            Attack();
        }
    }

    // Move towards the player
    private void HandleMovement()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    // Enemy attack logic
    public void Attack()
    {
        GameManager.Instance.HandleEnemyAttack(this);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Enemy died!");
            GameManager.Instance.RemoveEnemy(this);  // Notify GameManager to remove enemy
            Destroy(gameObject);
        }
    }
}
