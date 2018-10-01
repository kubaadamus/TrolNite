using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, 100), ForceMode.Impulse);
        Destroy(this.gameObject, 3);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
