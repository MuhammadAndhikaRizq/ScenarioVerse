using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    public MovementManager movementManager;
    public JumpManager jumpManager;
    public QuestManager questManager;

    private int isRunningHash;
    private int isJumpingHash;
    private InputControllers input;
    private Vector2 currentMovement;
    private bool movementPressed;
    private bool jumpPressed;

    void Awake()
    {
        input = new InputControllers();

        input.Player.Move.performed += OnMovePerformed;
        input.Player.Move.canceled += OnMoveCanceled;
        input.Player.Jump.performed += OnJumpPerformed;
        input.Player.Jump.canceled += OnJumpCanceled;
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
        animator = GetComponent<Animator>();
        isRunningHash = Animator.StringToHash("isRunning");
        isJumpingHash = Animator.StringToHash("isJumping");

        // Hide and lock the cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        jumpManager.Initialize(animator, isJumpingHash);

        // Ensure questManager is assigned
        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
        }

        if (questManager == null)
        {
            Debug.LogError("QuestManager is not assigned or found in the scene.");
        }

    }

    void Update()
    {
        HandleMovement();
        HandleJump();
    }

    void HandleMovement()
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

        if (movementPressed)
        {
            movementManager.Move(currentMovement);
        }
    }

    void HandleJump()
    {
         if (questManager == null)
        {
            Debug.LogError("QuestManager is not assigned.");
            return;
        }

        if (jumpPressed && questManager.GetQuestStep() >= 2) // Allow jumping only if the quest step is at least 2
        {
            jumpManager.Jump();

            if (questManager.GetQuestStep() == 2) // Complete the jump tutorial quest step
            {
                questManager.CompleteObjective();
                Debug.Log("Jump tutorial completed");
            }
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
