using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentGunPositionScript : MonoBehaviour {

    public GameObject Eyepos;
    public GameObject Hippos;
    public Camera Cam;
    int DestinationFieldOfView = 60;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1))
        {
            this.transform.position = Eyepos.transform.position;
            DestinationFieldOfView = 40;

        }
        if(Input.GetMouseButtonUp(1))
        {
            this.transform.position = Hippos.transform.position;
            DestinationFieldOfView = 60;
        }

        if(Cam.fieldOfView!=DestinationFieldOfView)
        {
            Cam.fieldOfView += (DestinationFieldOfView - Cam.fieldOfView) / 20.0f;
        }

	}
}
