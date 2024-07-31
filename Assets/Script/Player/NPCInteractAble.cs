using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractAble : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializeField] private string interactText;

    public void Interact()
    {
        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        dialogueManager.StartDialogue(dialogue);
        dialogueManager.OnDialogueEnd += EndInteraction;
    }

    public void EndInteraction()
    {
        PlayerInteract playerInteract = FindObjectOfType<PlayerInteract>();
        playerInteract.EndInteraction();
    }

    public string GetInteractText()
    {
        return interactText;
    }
}
