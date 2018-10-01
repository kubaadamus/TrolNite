using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoook : MonoBehaviour {

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axisn
    int GuiMessage = 0;
    public Character character;
    public Camera TPCamera;
    SpringJoint Joint = null;
    Rigidbody targetJoint;
    float Distance = 15;
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        Raycast();
        ChangeCamera();
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
            if (hit.collider.tag == "rifle" )
            {
                GuiMessage = 1;
                if(Input.GetKeyDown(KeyCode.F))
                {
                    if(!character.GunsList.Contains(GunType.rifle))
                    {
                        character.GunsList.Add(GunType.rifle);
                        Debug.Log("Podniesiono rajfla!");
                        character.Ammo[1] += 10;
                        Destroy(hit.collider.gameObject);
                        character.GunsCount = character.GunsList.Count;
                        Debug.Log("MASZ:" + character.GunsCount + " BRONI(E)");
                    }
                    else
                    {
                        Debug.Log("Masz juz rajfla! podniesiono ammo");
                        character.Ammo[1] += 10;
                        Destroy(hit.collider.gameObject);
                    }

                }
            }
            else if(hit.collider.tag == "pistol")
            {
                GuiMessage = 2;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (!character.GunsList.Contains(GunType.pistol))
                    {
                        character.GunsList.Add(GunType.pistol);
                        Debug.Log("Podniesiono pistola!");
                        character.Ammo[0] += 10;
                        Destroy(hit.collider.gameObject);
                        character.GunsCount = character.GunsList.Count;
                        Debug.Log("MASZ:" + character.GunsCount + " BRONI(E)");
                    }
                    else
                    {
                        Debug.Log("Masz juz pistola!, podniesiono ammo");
                        character.Ammo[0] += 10;
                        Destroy(hit.collider.gameObject);
                    }

                }
            }
            else
            {
                GuiMessage = 0;
            }
        }
    }
    void ChangeCamera()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            TPCamera.depth = 0;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TPCamera.depth = 1;
        }
    }
    private void OnGUI()
    {
        GUI.Button(new Rect(Screen.width / 2, Screen.height / 2, 30, 30), character.Ammo[character.SelectedGun].ToString());
        switch (GuiMessage)
        {
            case 1:
                GUI.Label(new Rect(Screen.width/2, Screen.height/2, 280, 20), "PICK UP RIFLE!");
                break;
            case 2:
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 280, 20), "PICK UP PISTOL!");
                break;

            default:
                GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 280, 20), "");
                break;
        }


    }
}
