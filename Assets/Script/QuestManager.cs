using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
    public TextMeshProUGUI questText;
    private int questStep = 0;

    private void Start()
    {
        UpdateQuestText();
    }

    public void CompleteObjective()
    {
        questStep++;
        Debug.Log("Objective completed. New quest step: " + questStep);
        UpdateQuestText();
    }

    public void UpdateQuestText()
    {
        switch (questStep)
        {
            case 0:
                questText.text = "Press 'E' to interact with the object.";
                break;
            case 1:
                questText.text = "Find the missing object and place it correctly.";
                break;
            case 2:
                questText.text = "Press space to jump. Press space 2x for double jump.";
                break;
            case 3:
                questText.text = "Go to the village.";
                break;
            default:
                questText.text = "Quest Completed!";
                break;
        }
        Debug.Log("Quest text updated: " + questText.text);
    }

    public int GetQuestStep()
    {
        return questStep;
    }

}
