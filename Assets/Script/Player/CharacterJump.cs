using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterJump : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;
    public float jumpForce = 5f;

    private int isJumpingHash;
    private Rigidbody rb;
    private InputControllers input;
    private bool jumpPressed;

    void Awake()
    {
        input = new InputControllers();

        // Menangani input lompat
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        jumpPressed = context.ReadValueAsButton();
    }

    void OnJumpCanceled(InputAction.CallbackContext context)
    {
        jumpPressed = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isJumpingHash = Animator.StringToHash("isJumping");
    }

    void Update()
    {
        HandleJump();
    }

    void HandleJump()
    {
        bool isJumping = animator.GetBool(isJumpingHash);

        if (jumpPressed && !isJumping)
        {
            // Set animation
            animator.SetBool(isJumpingHash, true);
            // Apply jump force
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        if (!jumpPressed && isJumping)
        {
            // Reset animation (typically you'd want a condition to check if actually landed)
            animator.SetBool(isJumpingHash, false);
        }
    }

    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
    }
}
