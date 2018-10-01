using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour {



    public GameObject Barrel;
    public GameObject Bullet;
    public GameObject Eyepos;
    public GameObject Hippos;
	void Start () {

	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            Instantiate(Bullet, Barrel.transform.position,Barrel.transform.rotation);
        }
        if(Input.GetMouseButtonDown(1))
        {
            transform.position = Eyepos.transform.position;
        }
        if (Input.GetMouseButtonUp(1))
        {
            transform.position = Hippos.transform.position;
        }
	}
}
