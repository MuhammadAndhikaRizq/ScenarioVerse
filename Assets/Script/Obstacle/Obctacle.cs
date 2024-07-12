using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obctacle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float xAngle = 0;
    [SerializeField] float yAngle = 0;
    [SerializeField] float zAngle = 0;
    void Update()
    {
        transform.Rotate(xAngle, yAngle, zAngle);
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            GetComponent<MeshRenderer>().material.color = Color.red;
        }   
    }
}
