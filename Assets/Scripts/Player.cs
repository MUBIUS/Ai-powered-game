using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    // public float mouseSensitivity = 1f;
    // private float xRotation = 0f; // For locking vertical rotation
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

    private CharacterController controller;
    // private float verticalRotation = 0f;
    private Vector3 velocity;
    private bool isGrounded;
    private WeaponManager weaponManager;
    private float nextUnarmedAttackTime = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the cursor in the center of the screen
        controller = GetComponent<CharacterController>();
        
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
    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeed;

        transform.Rotate(0, mouseX, 0);
        playerCamera.transform.Rotate(-mouseY, 0, 0);
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

    private void Die()
    {
        Debug.Log("Player died!");
        // Implement game over logic here
    }
}
