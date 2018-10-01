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
    SpringJoint Joint = null;
    Rigidbody targetJoint;
    float Distance = 2;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Raycast();
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        character.transform.rotation = Quaternion.Euler(character.transform.rotation.x, rotY, character.transform.rotation.z);
        transform.rotation = localRotation;

        if (Input.GetMouseButtonUp(1))
        {
            if (Joint != null)
            {
                Destroy(Joint);
                Joint = null;
            }
        }
    }
    void Raycast()
    {

        RaycastHit hit;
        Ray landingRay = new Ray(transform.position,transform.forward);
        Debug.DrawRay(transform.position,transform.forward * Distance);

        //STRZELANIE RAYCASTEM
        if (Physics.Raycast(landingRay, out hit, Distance))
        {
            if (hit.collider.tag == "gripable" && Input.GetMouseButtonDown(0))
            {
                if (Joint == null)
                {
                    Joint = gameObject.AddComponent(typeof(SpringJoint)) as SpringJoint;
                    Joint.anchor = new Vector3(0, 0, 0);
                    Joint.autoConfigureConnectedAnchor = false;
                    Joint.connectedAnchor = new Vector3(0, 0, 0);
                    Joint.minDistance = 1.0f;
                    Joint.maxDistance = 2.0f;
                    Debug.Log("GRIP XD");
                    targetJoint = hit.rigidbody;
                    Joint.connectedBody = targetJoint;
                    Joint.enableCollision = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                    GetComponent<Rigidbody>().useGravity = false;

                }


            }
            if (hit.collider.tag == "")
            {
            }
        }
    }
}
