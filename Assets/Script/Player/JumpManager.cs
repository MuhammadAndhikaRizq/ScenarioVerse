using UnityEngine;

public class JumpManager : MonoBehaviour
{
    private Rigidbody rb;
    private int jumpCount = 0;
    public float jumpForce = 5f;
    public int maxJumps = 2; // For double jump
    public QuestManager questManager;
    private Animator animator;
    private int isJumpingHash;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Find the QuestManager if not assigned in the inspector
        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
            if (questManager == null)
            {
                Debug.LogError("QuestManager script not found!");
            }
        }
    }

    public void Initialize(Animator animator, int isJumpingHash)
    {
        this.animator = animator;
        this.isJumpingHash = isJumpingHash;
    }

    public void Jump()
    {
        if (jumpCount < maxJumps)
        {
            animator.SetBool(isJumpingHash, true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpCount++;

            // Update the quest step if jumping for the first time
            if (jumpCount == 1 && questManager != null && questManager.GetQuestStep() == 2)
            {
                questManager.CompleteObjective();
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        jumpCount = 0;
        animator.SetBool(isJumpingHash, false);
    }
}
