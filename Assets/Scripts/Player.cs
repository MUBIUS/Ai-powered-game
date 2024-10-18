using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public float speed = 5f;
    public float attackRange = 2f;
    public int attackDamage = 20;

    public Camera playerCamera; // Reference to the player's camera
    public float lookSpeed = 2f; // Speed of the mouse look

    private void Start()
    {
        // Hide and lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();

        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
    }

    // Handle player movement
    private void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized;

        if (movement.magnitude > 0)
        {
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }
    }

    // Handle mouse look
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(0, mouseX, 0);
        playerCamera.transform.Rotate(-mouseY, 0, 0);
    }

    // Player attack logic
    public void Attack()
    {
        // Cast a sphere to detect enemies in attack range
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (Collider hitCollider in hitColliders)
        {
            Enemy enemy = hitCollider.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Deal damage to the enemy
                enemy.TakeDamage(attackDamage);
                Debug.Log("Enemy hit for " + attackDamage + " damage!");
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Player died!");
            // Handle player death (e.g., reload scene)
        }
    }

    // Optional: To visualize the attack range in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
