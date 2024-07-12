using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    public Animator animator;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    private int isRunningHash;
    private Rigidbody rb;
    private InputControllers input;
    private Vector2 currentMovement;
    private Vector2 currentLook;
    private bool movementPressed;

    void Awake()
    {
        input = new InputControllers();

        input.Player.Move.performed += OnMovePerformed;
        input.Player.Move.canceled += OnMoveCanceled;
        input.Player.Look.performed += OnLookPerformed;
        input.Player.Look.canceled += OnLookCanceled;
    }

    void OnMovePerformed(InputAction.CallbackContext context)
    {
        currentMovement = context.ReadValue<Vector2>();
        movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
    }

    void OnMoveCanceled(InputAction.CallbackContext context)
    {
        currentMovement = Vector2.zero;
        movementPressed = false;
    }

    void OnLookPerformed(InputAction.CallbackContext context)
    {
        currentLook = context.ReadValue<Vector2>();
    }

    void OnLookCanceled(InputAction.CallbackContext context)
    {
        currentLook = Vector2.zero;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");

        // Menyembunyikan dan mengunci kursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        bool isRunning = animator.GetBool(isRunningHash);

        if (movementPressed && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (!movementPressed && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

        Vector3 movement = new Vector3(currentMovement.x, 0, currentMovement.y);
        movement = transform.TransformDirection(movement) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }

    void Rotate()
    {
        if (currentLook.sqrMagnitude > 0.01f)
        {
            Vector3 targetDirection = new Vector3(currentLook.x, 0, currentLook.y);
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
