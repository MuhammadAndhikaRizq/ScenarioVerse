using System.Collections;
using UnityEngine;

public class BridgeActivation : MonoBehaviour
{
    public GameObject bridgeObject;
    public Transform startTransform; // Initial position of the bridge (hidden)
    public Transform endTransform; // Final position of the bridge (visible)
    public float moveSpeed = 3f; // Speed of bridge movement
    public bool requiresItem; // Determines if the button requires an item to be activated
    public bool bridgeActivated = false; // Status of the bridge, whether at the final position or not
    private bool isMoving = false; // Status of bridge movement
    private PlayerPickupandDrop currentItem; // Reference to the current item
    private Transform player;
    private float distanceToPlayer;

    public BridgeActivation otherBridge; // Reference to the other bridge
    public QuestManager questManager; // Reference to the QuestManager

    void Start()
    {
        player = GameObject.Find("Ninja")?.transform;
        if (player == null)
        {
            Debug.LogError("Player object not found!");
        }

        currentItem = FindObjectOfType<PlayerPickupandDrop>();
        if (currentItem == null)
        {
            Debug.LogError("PlayerPickupandDrop script not found!");
        }

        if (bridgeObject != null && startTransform != null)
        {
            bridgeObject.transform.position = startTransform.position;
        }
        else
        {
            Debug.LogError("BridgeObject or StartTransform is not assigned!");
        }

        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
            if (questManager == null)
            {
                Debug.LogError("QuestManager script not found!");
            }
        }
    }

    void Update()
    {
        if (requiresItem)
        {
            CheckItemCondition();
        }
        else
        {
            CheckPlayerInteraction();
        }
    }

    void CheckItemCondition()
    {
        if (player == null || currentItem == null) return;

        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= 2f && currentItem.isItemOnButton && Input.GetKeyDown(KeyCode.E) && !isMoving)
        {
            ActivateBridge();
            Debug.Log("Bridge activated with item.");

            if (otherBridge != null && otherBridge.bridgeActivated)
            {
                otherBridge.ResetBridge();
                Debug.Log("Other bridge reset.");
            }
        }
    }

    void CheckPlayerInteraction()
    {
        if (player == null) return;

        distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= 1.5f && !isMoving && Input.GetKeyDown(KeyCode.E))
        {
            ToggleBridge();
            Debug.Log("Bridge toggled by player interaction.");

            // Reset other bridge if exists
            if (otherBridge != null && otherBridge.bridgeActivated)
            {
                otherBridge.ResetBridge();
                Debug.Log("Other bridge reset.");
            }
        }
    }

    void ToggleBridge()
    {
        if (bridgeObject == null || startTransform == null || endTransform == null) return;

        StartCoroutine(MoveBridge(bridgeActivated ? endTransform.position : startTransform.position,
                                  bridgeActivated ? startTransform.position : endTransform.position));
        bridgeActivated = !bridgeActivated;
        Debug.Log("Bridge toggled. New state: " + (bridgeActivated ? "Activated" : "Deactivated"));

        // Update quest step when bridge is toggled
        if (questManager != null)
        {
            questManager.CompleteObjective();
        }
    }

    public void ActivateBridge()
    {
        if (bridgeObject == null || startTransform == null || endTransform == null) return;

        if (!bridgeActivated) // Only toggle if not already activated
        {
            StartCoroutine(MoveBridge(startTransform.position, endTransform.position));
            bridgeActivated = true;
            Debug.Log("Bridge activated.");

            // Update quest step when bridge is activated
            if (questManager != null)
            {
                questManager.CompleteObjective();
            }
        }

        // Reset other bridge if exists
        if (otherBridge != null && otherBridge.bridgeActivated)
        {
            otherBridge.ResetBridge();
            Debug.Log("Other bridge reset.");
        }
    }

    public void ResetBridge()
    {
        if (!isMoving && bridgeObject != null && startTransform != null)
        {
            StartCoroutine(MoveBridge(bridgeObject.transform.position, startTransform.position));
            bridgeActivated = false;
            Debug.Log("Bridge reset.");
        }
    }

    private IEnumerator MoveBridge(Vector3 startPos, Vector3 endPos)
    {
        if (bridgeObject == null) yield break;

        isMoving = true;
        float journeyLength = Vector3.Distance(startPos, endPos);
        float startTime = Time.time;

        while (Vector3.Distance(bridgeObject.transform.position, endPos) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * moveSpeed;
            float fractionOfJourney = distCovered / journeyLength;
            bridgeObject.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney);
            yield return null;
        }

        bridgeObject.transform.position = endPos;
        isMoving = false;
        Debug.Log("Bridge movement completed.");
    }
}
