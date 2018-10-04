using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LolScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animation>().Stop();
            GetComponent<Animation>().Play("AtakHeheszka");
        }
	}
}
