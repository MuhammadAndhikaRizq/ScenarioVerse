using UnityEngine;
using System.Collections.Generic;

public class PlayerInteract : MonoBehaviour
{
    public bool isInteracting = false;
    private PlayerController playerController;
    private Collider[] colliderBuffer = new Collider[10]; // Buffer untuk OverlapSphereNonAlloc

    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (!isInteracting && Input.GetKeyDown(KeyCode.E))
        {
            NPCInteractAble closestNPC = GetInteractableObject();
            if (closestNPC != null)
            {
                StartInteraction();
                closestNPC.Interact();
            }
        }
    }

    public NPCInteractAble GetInteractableObject()
    {
        float interactRange = 2f; 
        int numColliders = Physics.OverlapSphereNonAlloc(transform.position, interactRange, colliderBuffer);

        NPCInteractAble closestNPC = null;
        float closestDistance = Mathf.Infinity;

        for (int i = 0; i < numColliders; i++)
        {
            Collider collider = colliderBuffer[i];
            if (collider.TryGetComponent(out NPCInteractAble npc))
            {
                float distance = Vector3.Distance(transform.position, npc.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNPC = npc;
                }
            }
        }

        return closestNPC;
    }

    public void StartInteraction()
    {
        isInteracting = true;
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    public void EndInteraction()
    {
        isInteracting = false;
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
