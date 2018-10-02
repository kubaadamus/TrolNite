using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour {


    Vector3 PosDown;
    Vector3 PosUp;
    Vector3 Destination;

    void Start () {
        Destination = transform.position;
        PosDown = transform.position;
        PosUp = transform.position + new Vector3(0,2,0);

	}
	
	// Update is called once per frame
	void Update () {
        if(transform.position!=Destination)
        {
            transform.position += (Destination - transform.position) / 100.0f;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destination = PosUp;
            CharacterController PlayerController = other.GetComponent<CharacterController>();
            PlayerController.skinWidth = 0.01f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destination = PosDown;
            CharacterController PlayerController = other.GetComponent<CharacterController>();
            PlayerController.skinWidth = 0.08f;
        }
    }
    private void OnTriggerStay(Collider other)
    {

    }
}
