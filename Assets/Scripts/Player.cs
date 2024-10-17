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
        Cursor.visible = false; // Optional: Make the cursor invisible
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

        // Create movement vector relative to the camera's forward direction
        Vector3 forward = playerCamera.transform.forward; // Get camera forward direction
        Vector3 right = playerCamera.transform.right; // Get camera right direction

        // Flatten the vectors to the XZ plane
        forward.y = 0; 
        right.y = 0; 

        forward.Normalize();
        right.Normalize();

        // Calculate movement based on input
        Vector3 movement = (forward * moveVertical + right * moveHorizontal).normalized; // Normalize to ensure consistent speed

        if (movement.magnitude > 0)
        {
            // Move the player based on input
            transform.Translate(movement * speed * Time.deltaTime, Space.World);
        }
    }

    // Handle mouse look to rotate the player
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        // Rotate the player around the Y-axis
        transform.Rotate(0, mouseX, 0);

        // Rotate the camera around the X-axis (for looking up and down)
        playerCamera.transform.Rotate(-mouseY, 0, 0);

        // Optional: Clamp camera rotation to prevent flipping
        // Implement camera clamping logic if needed here
    }

    // Player attack logic
    public void Attack()
    {
        GameManager.Instance.HandlePlayerAttack(this);
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
}
