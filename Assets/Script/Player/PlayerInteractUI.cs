using UnityEngine;
using TMPro;

public class PlayerInteractUI : MonoBehaviour
{
    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private void Update()
    {
        NPCInteractAble interactableNPC = playerInteract.GetInteractableObject();
        if (interactableNPC != null && !playerInteract.isInteracting)
        {
            Show(interactableNPC);
        }
        else
        {
            Hide();
        }
    }

    private void Show(NPCInteractAble npcInteractAble)
    {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = npcInteractAble.GetInteractText();
    }

    private void Hide()
    {
        containerGameObject.SetActive(false);
    }
}
