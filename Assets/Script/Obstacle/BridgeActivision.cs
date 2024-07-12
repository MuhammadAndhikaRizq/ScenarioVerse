using System.Collections;
using UnityEngine;

public class BridgeActivision : MonoBehaviour
{
    public GameObject bridgeObject;
    public Transform startTransform; // Posisi awal jembatan (tersembunyi)
    public Transform endTransform; // Posisi akhir jembatan (terlihat)
    private Transform player;
    public float moveSpeed = 3f; // Kecepatan gerakan jembatan
    private bool bridgeActivated = false; // Status jembatan, apakah di posisi akhir atau tidak
    private bool isMoving = false; // Status pergerakan jembatan

    void Start()
    {
        player = GameObject.Find("Ninja").transform;
        if (player == null)
        {
            Debug.LogError("Player object not found!");
        }

        if (startTransform == null || endTransform == null)
        {
            Debug.LogError("Start or End transform is not set!");
        }

        // Set posisi awal jembatan
        bridgeObject.transform.position = startTransform.position;
    }

    void Update()
    {
        InteractionPlayer();
    }

    void InteractionPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= 1.5f && !isMoving)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (bridgeActivated)
                {
                    StartCoroutine(MoveBridge(endTransform.position, startTransform.position));
                }
                else
                {
                    StartCoroutine(MoveBridge(startTransform.position, endTransform.position));
                }
                bridgeActivated = !bridgeActivated;
            }
        }
    }

    private IEnumerator MoveBridge(Vector3 startPos, Vector3 endPos)
    {
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
    }
}
