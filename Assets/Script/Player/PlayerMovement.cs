using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Map Boundary (X/Z)")]
    public Vector2 mapMinXZ = new Vector2(-48f, -48f);
    public Vector2 mapMaxXZ = new Vector2(148f, 148f);

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    [SerializeField] VirtualJoystick mobileJoystick;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool jumpRequested;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Get input
        Vector2 movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (mobileJoystick != null && mobileJoystick.isActiveAndEnabled && mobileJoystick.Direction.sqrMagnitude > 0f)
            movementInput = mobileJoystick.Direction;

        // Move
        Vector3 move = transform.right * movementInput.x + transform.forward * movementInput.y;

        // Sprint
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        Vector3 horizontalTarget = transform.position + move * currentSpeed * Time.deltaTime;
        horizontalTarget.x = Mathf.Clamp(horizontalTarget.x, mapMinXZ.x, mapMaxXZ.x);
        horizontalTarget.z = Mathf.Clamp(horizontalTarget.z, mapMinXZ.y, mapMaxXZ.y);
        controller.Move(horizontalTarget - transform.position);

        // Jump
        bool jumpPressed = Input.GetButtonDown("Jump") || jumpRequested;
        jumpRequested = false;
        if (jumpPressed && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void RequestJump()
    {
        jumpRequested = true;
    }
}
