using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public Transform player;
    public BridgeActivation[] bridges;

    void Update()
    {
        // Cek input untuk menyimpan permainan
        if (Input.GetKeyDown(KeyCode.F))
        {
            SaveGame();
            Debug.Log("Game saved!");
        }

        // Cek input untuk memuat permainan
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
            Debug.Log("Game loaded!");
        }
    }

    public void SaveGame()
    {
        GameData data = new GameData
        {
            playerPosX = player.position.x,
            playerPosY = player.position.y,
            playerPosZ = player.position.z
        };

        foreach (BridgeActivation bridge in bridges)
        {
            BridgeData bridgeData = new BridgeData
            {
                bridgeName = bridge.name,
                bridgePosX = bridge.bridgeObject.transform.position.x,
                bridgePosY = bridge.bridgeObject.transform.position.y,
                bridgePosZ = bridge.bridgeObject.transform.position.z,
                bridgeActivated = bridge.bridgeActivated
            };
            data.bridges.Add(bridgeData);
        }

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("GameData", jsonData);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("GameData"))
        {
            string jsonData = PlayerPrefs.GetString("GameData");
            GameData data = JsonUtility.FromJson<GameData>(jsonData);

            player.position = new Vector3(data.playerPosX, data.playerPosY, data.playerPosZ);

            foreach (BridgeData bridgeData in data.bridges)
            {
                foreach (BridgeActivation bridge in bridges)
                {
                    if (bridge.name == bridgeData.bridgeName)
                    {
                        bridge.bridgeObject.transform.position = new Vector3(bridgeData.bridgePosX, bridgeData.bridgePosY, bridgeData.bridgePosZ);
                        bridge.bridgeActivated = bridgeData.bridgeActivated;
                        break;
                    }
                }
            }
        }
    }
}
