using UnityEngine;
using UnityEngine.SceneManagement;
public class Player : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float mouseSensitivity = 1f;
    private float xRotation = 0f; // For locking vertical rotation
    public float lookSpeed = 2f; // Speed of the mouse look
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0f;
    public LayerMask groundMask;

    [Header("Combat")]
    public int health = 100;
    public float pickupRange = 3f;
    public float unarmedDamage = 5f;
    public float unarmedRange = 2f;
    public float unarmedAttackRate = 1f;

    [Header("References")]
    public Camera playerCamera;
    public Player player;

    private CharacterController controller;
    // private float verticalRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private WeaponManager weaponManager;
    private float nextUnarmedAttackTime = 0f;

    void Start()
{
    Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
    controller = GetComponent<CharacterController>();

    // Initialize weaponManager
    weaponManager = GetComponent<WeaponManager>();
}

    private void Update()
    {
        HandleMovement();
        HandleMouseLook();
        HandleJump();
        HandleCombat();
        HandleWeaponPickup();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravity Handling
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // This is for precise mouse look control with no sliding/spinning
    // Handle mouse look
    void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Calculate vertical rotation (for looking up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Clamp to avoid over-rotation

        // Apply rotations
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); // Vertical rotation
        playerCamera.transform.Rotate(Vector3.up * mouseX); // Horizontal rotation (rotating the player body)
    }

    void HandleJump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Reset y velocity if grounded
        }

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCombat()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (weaponManager.CurrentWeapon != null)
            {
                weaponManager.CurrentWeapon.Fire();
            }
            else
            {
                UnarmedAttack();
            }
        }
    }

    void UnarmedAttack()
    {
        if (Time.time >= nextUnarmedAttackTime)
        {
            nextUnarmedAttackTime = Time.time + 1f / unarmedAttackRate;

            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, unarmedRange))
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage((int)unarmedDamage);
                    Debug.Log("Unarmed attack hit enemy for " + unarmedDamage + " damage!");
                }
            }
        }
    }

    void HandleWeaponPickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, pickupRange))
            {
                WeaponPickup weaponPickup = hit.transform.GetComponent<WeaponPickup>();
                if (weaponPickup != null)
                {
                    weaponManager.EquipWeapon(weaponPickup.WeaponPrefab);
                    Destroy(weaponPickup.gameObject);
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player took " + damage + " damage! Remaining health: " + health);

        if (health <= 0)
        {
            Die();
        }
    }

    public GameObject gameOverPanel; // Drag and drop the panel in the Inspector

    private void Die()
    {
        Debug.Log("Player died!");
        
        // Stop the game by freezing time
        Time.timeScale = 0f; // Freeze time

        // Show the Game Over panel
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;

        Cursor.visible = true;
    }
    
    public void RetryGame()
    {
        // Unfreeze time and reload the current scene
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Reload current scene
    }

    public void ReturnToMenu()
    {
        // Unfreeze time and load the menu scene
        Time.timeScale = 1f; // Resume time
        SceneManager.LoadScene("Menu"); // Load menu scene, replace "MenuScene" with the actual scene name
    }
}
