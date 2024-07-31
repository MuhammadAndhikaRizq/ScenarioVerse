using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public QuestManager questManager;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "Ninja" && questManager.GetQuestStep() == 3)
        {
            questManager.CompleteObjective();
        }
    }
}
