using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{

    public float inputSensitivity;
    public float rotationSpeed;

    private InputAction move;
    private Rigidbody2D rigidbodyComponent;
    private Vector2 movementInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Using Unitys new input system, where the "Move" action represents the players movement input, i.e. WASD, arrow keys or a joystick.
        move = InputSystem.actions.FindAction("Move");
        // Instantiate the rigidbodyComponent variable. I have it in a variable because that makes it more refactorable and quicker than calling the function every time.
        rigidbodyComponent = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Read the last input from the user.
        movementInput = move.ReadValue<Vector2>();
        if (movementInput != Vector2.zero)
        {
            float targetLookAngle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg - 90;
            // This interpolates between the last updates rotation direction and the target rotation to smoothly transition.
            Quaternion lookRotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, targetLookAngle), rotationSpeed);
            transform.rotation = lookRotation;
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputFixed = inputSensitivity * Time.fixedDeltaTime * movementInput;
        rigidbodyComponent.linearVelocity += inputFixed;
    }
}
