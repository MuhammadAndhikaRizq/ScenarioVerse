using UnityEngine;

public class CharacterPickupDrop : MonoBehaviour
{
    private Transform PickUpPoint;
    private Transform player;
    public float pickUpDistance;
    public float force;
    public bool readyThrow;
    public bool itemIsPickUp;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Ninja").transform;
        PickUpPoint = GameObject.Find("PickUpPoint").transform;
    }

    void Update()
    {
        PickUp();
        Throw();
        
    }

    void PickUp()
    {
        if (Input.GetKey(KeyCode.E) && itemIsPickUp == true && readyThrow)
        {
            force += 300 * Time.deltaTime;
        }

        pickUpDistance = Vector3.Distance(player.position, transform.position);

        if(pickUpDistance <= 2)
        {
            if (Input.GetKeyDown(KeyCode.E) && itemIsPickUp == false && PickUpPoint.childCount < 1)
            {
                GetComponent<Rigidbody>().useGravity = false;
                GetComponent<BoxCollider>().enabled = false;
                this.transform.position = PickUpPoint.position;
                this.transform.parent = GameObject.Find("PickUpPoint").transform;

                itemIsPickUp = true;
                force = 0;
            }
        }
    }

    void Throw()
    {
        if(Input.GetKeyUp(KeyCode.E) && itemIsPickUp == true)
        {
            readyThrow = true;
            if(force > 10)
            {
                rb.AddForce(player.transform.forward * force);
                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = true;
                itemIsPickUp = false;

                force = 0;
                readyThrow = false;
            }
            
            force = 0;
        }
    }
}
