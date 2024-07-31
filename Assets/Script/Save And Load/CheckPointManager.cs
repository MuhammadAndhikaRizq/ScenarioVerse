using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public Transform player;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = player.position;
        LoadCheckpoint();
    }

    void Update()
    {
        if (player.position.y < -15f)
        {
            ResetToCheckpoint();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetToInitialPosition();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            SaveCheckpoint(other.transform.position);
            Debug.Log("Checkpoint reached and saved: " + other.transform.position);
        }
    }

    void SaveCheckpoint(Vector3 checkpointPosition)
    {
        PlayerPrefs.SetFloat("CheckpointX", checkpointPosition.x);
        PlayerPrefs.SetFloat("CheckpointY", checkpointPosition.y);
        PlayerPrefs.SetFloat("CheckpointZ", checkpointPosition.z);
        PlayerPrefs.Save();
    }

    void LoadCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            float x = PlayerPrefs.GetFloat("CheckpointX");
            float y = PlayerPrefs.GetFloat("CheckpointY");
            float z = PlayerPrefs.GetFloat("CheckpointZ");
            player.position = new Vector3(x, y, z);
            Debug.Log("Loaded checkpoint: " + player.position);
        }
    }

    void ResetToCheckpoint()
    {
        if (PlayerPrefs.HasKey("CheckpointX"))
        {
            LoadCheckpoint();
        }
        else
        {
            player.position = initialPosition;
        }
    }

    void ResetToInitialPosition()
    {
        player.position = initialPosition;
        PlayerPrefs.DeleteKey("CheckpointX");
        PlayerPrefs.DeleteKey("CheckpointY");
        PlayerPrefs.DeleteKey("CheckpointZ");
        PlayerPrefs.Save();
    }
}
