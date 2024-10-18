using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public int health = 50;
    public int attackDamage = 10;
    public float attackRange = 2f; // Range for the enemy attack
    public float learningRate = 0.1f;
    public float discountFactor = 0.9f;

    public QLearningAgent qLearningAgent;
    private int lastState;
    private int lastAction;
    private Player player;

    private void Start()
    {
        qLearningAgent = new QLearningAgent(learningRate, discountFactor);
        player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        int currentState = GetCurrentState();
        int action = qLearningAgent.ChooseAction(currentState);

        PerformAction(action);
        lastState = currentState;
        lastAction = action;
    }

    private int GetCurrentState()
    {
        // You could base this on the player's position, distance, or other factors
        float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        return distanceToPlayer < attackRange ? 1 : 0;  // Simple example: 0 = far, 1 = close
    }

    private void PerformAction(int action)
    {
        switch (action)
        {
            case 0: MoveTowardsPlayer(); break;
            case 1: AttackPlayer(); break;
            case 2: Retreat(); break;
        }
    }

    private void MoveTowardsPlayer()
    {
        // Logic for moving towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, Time.deltaTime * 2);
    }

    private void AttackPlayer()
    {
        // Check if the player is within attack range
        if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
        {
            // Damage the player
            player.TakeDamage(attackDamage);
            Debug.Log("Enemy attacked the player!");
        }
    }

    private void Retreat()
    {
        // Logic for retreating from the player
        transform.position = Vector3.MoveTowards(transform.position, -player.transform.position, Time.deltaTime * 2);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        qLearningAgent.UpdateQTable(lastState, lastAction, -10, GetCurrentState());
        GameManager.Instance.OnEnemyKilled();
        Destroy(gameObject);
    }
}
