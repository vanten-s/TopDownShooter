using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerScript : MonoBehaviour
{

    public float inputSensitivity;
    public float rotationSpeed;
    public float bulletSpeed;
    public float cameraSpeed;
    public GameObject bullet;
    public event EventHandler<int> OnAttack;
    
    // The `health` variable is implemented as property since we never want to update it without also triggering OnAttack
    public int health
    {
        get
        {
            return _internalHealth;
        }
        set
        {
            _internalHealth = value; 
            if (value == 0)
            {
                Destroy(gameObject);
            }
            OnAttack(this, _internalHealth);
        }
    }

    private InputAction move;
    private InputAction look;
    private InputAction attack;
    private bool attackHeld;
    private Rigidbody2D rigidbodyComponent;
    private Vector2 movementInput;
    private Camera mainCamera;

    // Should never be accessed outside of `health` property
    private int _internalHealth = 3;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Using Unitys new input system, where the "Move" action represents the players movement input, i.e. WASD, arrow keys or a joystick.
        move = InputSystem.actions.FindAction("Move");
        attack = InputSystem.actions.FindAction("Attack");
        look = InputSystem.actions.FindAction("Look");
        // Instantiate the rigidbodyComponent variable. I have it in a variable because it's quicker than calling the function multiple times.
        rigidbodyComponent = GetComponent<Rigidbody2D>();
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (attack.IsPressed())
        {
            // attackHeld makes sure that the user releases the attack button before shooting again.
            if (!attackHeld)
            {
                Shoot();
            }
            attackHeld = true;
        } else
        {

            attackHeld = false;
        }
        // Read the last input from the user.
        movementInput = move.ReadValue<Vector2>();
        Vector2 lookDirection = look.ReadValue<Vector2>();

        float mouseX = Mouse.current.position.x.value;
        float mouseY = Mouse.current.position.y.value;

        // Because the mouse position is in pixels, we want to project it from the "screen" position to a "world" position. 
        Vector3 worldPoint = mainCamera.ScreenToWorldPoint(new Vector2(mouseX, mouseY));
        // Then we subtract the players position to get the point relative to the 
        Vector2 relativePosition = worldPoint - transform.position;

        float targetLookAngle = Mathf.Atan2(relativePosition.y, relativePosition.x) * Mathf.Rad2Deg - 90;
        transform.eulerAngles = new(0, 0, targetLookAngle);
    }

    private void FixedUpdate()
    {
        Vector2 inputFixed = inputSensitivity * Time.fixedDeltaTime * movementInput;
        rigidbodyComponent.linearVelocity += inputFixed;

        ClampPosition();
    }

    void Shoot()
    {
        GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
        Vector2 rotationVector = new(Mathf.Sin(-transform.eulerAngles.z * Mathf.Deg2Rad), Mathf.Cos(-transform.eulerAngles.z * Mathf.Deg2Rad));
        bulletInstance.GetComponent<Rigidbody2D>().linearVelocity = rigidbodyComponent.linearVelocity + rotationVector * bulletSpeed;
        Destroy(bulletInstance, 3);
    }

    void ClampPosition()
    {
        // This code figures out the camera bounds, and then makes sure that if the player is outside of the bounds, it gets looped around the other side of the screen.
        Vector2 upperRightCorner = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
        Vector2 lowerLeftCorner = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));

        float gameHeight = upperRightCorner.y - lowerLeftCorner.y;
        float gameWidth = upperRightCorner.x - lowerLeftCorner.x;

        if (transform.position.y > upperRightCorner.y)
        {
            transform.position = transform.position - new Vector3(0, gameHeight);
        }

        if (transform.position.y < lowerLeftCorner.y)
        {
            transform.position = transform.position + new Vector3(0, gameHeight);
        }

        if (transform.position.x > upperRightCorner.x)
        {
            transform.position = transform.position - new Vector3(gameWidth, 0);
        }

        if (transform.position.x < lowerLeftCorner.x)
        {
            transform.position = transform.position + new Vector3(gameWidth, 0);
        }
    }
}
