using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData
{
    public float playerPosX;
    public float playerPosY;
    public float playerPosZ;

    public List<BridgeData> bridges = new List<BridgeData>();
}
[System.Serializable]
public class BridgeData
{
    public string bridgeName;
    public float bridgePosX;
    public float bridgePosY;
    public float bridgePosZ;
    public bool bridgeActivated;
}

