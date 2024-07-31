using UnityEngine;

public class PlayerPickupandDrop : MonoBehaviour
{
    private Transform PickUpPoint;
    private Transform player;
    private Transform ButtonPoint;
    public float pickUpDistance;
    public float dropDistance;
    public float force;
    public bool readyThrow;
    public bool itemIsPickUp;
    public bool buttonIsStick;

    public QuestManager questManager;
    private Rigidbody rb;
    public bool isItemOnButton = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.Find("Ninja")?.transform;
        if (player == null)
        {
            Debug.LogError("Player object (Ninja) not found!");
        }

        PickUpPoint = GameObject.Find("PickUpPoint")?.transform;
        if (PickUpPoint == null)
        {
            Debug.LogError("PickUpPoint object not found!");
        }

        ButtonPoint = GameObject.Find("ButtonPoint")?.transform;
        if (ButtonPoint == null)
        {
            Debug.LogError("ButtonPoint object not found!");
        }

        if (questManager == null)
        {
            questManager = FindObjectOfType<QuestManager>();
            if (questManager == null)
            {
                Debug.LogError("QuestManager script not found in the scene!");
            }
        }
    }

    void Update()
    {
        PickUp();
        Throw();
        ThrowPoint();
    }

    void PickUp()
    {
        if (Input.GetKey(KeyCode.E) && itemIsPickUp == true && readyThrow)
        {
            force += 300 * Time.deltaTime;
        }

        if (player == null) return;

        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if (pickUpDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && itemIsPickUp == false && PickUpPoint.childCount < 1 && buttonIsStick == false && questManager != null && questManager.GetQuestStep() == 1)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                GetComponent<BoxCollider>().enabled = false;
                transform.position = PickUpPoint.position;
                transform.parent = PickUpPoint;

                itemIsPickUp = true;
                force = 0;
            }
        }
    }

    void Throw()
    {
        if (Input.GetKeyUp(KeyCode.E) && itemIsPickUp == true)
        {
            readyThrow = true;
            if (force > 10)
            {
                rb.AddForce(player.transform.forward * force);
                transform.parent = null;
                rb.useGravity = true;
                rb.isKinematic = false;
                GetComponent<BoxCollider>().enabled = true;
                itemIsPickUp = false;

                force = 0;
                readyThrow = false;

                RemoveItemFromButton();
            }

            force = 0;
        }
    }

    void ThrowPoint()
    {
        if (ButtonPoint == null) return;

        dropDistance = Vector3.Distance(ButtonPoint.position, transform.position);
        if (dropDistance <= 1.5f)
        {

            if (Input.GetKeyDown(KeyCode.E) && itemIsPickUp == true && ButtonPoint.childCount < 1)
            {
                rb.useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
                transform.position = ButtonPoint.position;
                transform.parent = ButtonPoint;

                buttonIsStick = true;
                force = 0;

                isItemOnButton = true;

                var buttonActivation = FindObjectOfType<BridgeActivation>();
                if (buttonActivation != null)
                {
                    buttonActivation.ActivateBridge();
                }
            }
        }
    }

    public void RemoveItemFromButton()
    {
        buttonIsStick = false;
        isItemOnButton = false;
        transform.parent = null;
    }
}
