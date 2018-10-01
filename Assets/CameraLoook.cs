using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoook : MonoBehaviour {

    //STEROWANIE
    bool MouseLook = true;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axisn
    public GameObject character;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, rotY, character.transform.rotation.z);
        transform.rotation = localRotation;
    }
}
