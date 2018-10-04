using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastDebug : MonoBehaviour {


    

   
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        
        Debug.DrawRay(transform.position, transform.forward * 1000);


    }
}
