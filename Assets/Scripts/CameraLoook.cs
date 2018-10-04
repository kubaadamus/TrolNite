using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLoook : MonoBehaviour
{

    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axisn
    public string GuiMessage = "";
    public Character character;
    public Camera TPCamera;
    SpringJoint Joint = null;
    Rigidbody targetJoint;
    float Distance = 15;
    public GameObject HandPosition;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (Joint != null)
            {
                Destroy(Joint);
                Joint = null;
            }
        }
        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            transform.localPosition = new Vector3(0, 2, 0);
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            transform.localPosition = new Vector3(0, 5.24f, 0);
        }
    }
    void Raycast()
    {

        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward * Distance);

        //STRZELANIE RAYCASTEM
        if (Physics.Raycast(landingRay, out hit, Distance))
        {
            if (hit.collider.tag == "gripable" || hit.collider.tag == "NPC")
            {
                GuiMessage = "Grab "+hit.collider.name.ToString();





                if (Input.GetKey(KeyCode.F) && Joint == null)
                {
                    if (hit.collider.tag == "NPC")
                    {
                        hit.collider.gameObject.GetComponent<npcMove>().DestroyNavMesh();
                    }
                    Joint = gameObject.AddComponent(typeof(SpringJoint)) as SpringJoint;
                    Joint.anchor = HandPosition.transform.localPosition;
                    Joint.autoConfigureConnectedAnchor = false;
                    Joint.connectedAnchor = new Vector3(0, 0, 0);

                    Joint.minDistance = 0.0f;
                    Joint.maxDistance = 0.1f;
                    Joint.spring = 80;
                    Joint.damper = 2;

                    Debug.Log("GRIP XD");
                    targetJoint = hit.rigidbody;
                    Joint.connectedBody = targetJoint;
                    Joint.enableCollision = true;
                    GetComponent<Rigidbody>().isKinematic = true;
                    GetComponent<Rigidbody>().useGravity = false;
                }
            }
            else if (hit.collider.tag == "shotgun")
            {
                GuiMessage = "Grab SHOTGUN";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (!character.GunsList.Contains(GunType.shotgun))
                    {
                        character.GunsList.Add(GunType.shotgun);
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
            else if (hit.collider.tag == "pistol")
            {
                GuiMessage = "Grab PISTOL";
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
            else if (hit.collider.tag == "shotgunAmmo")
            {
                GuiMessage = "Grab SHOTGUN AMMO";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Podniesiono shotgun Ammo");
                    character.Ammo[1] += 10;
                    Destroy(hit.collider.gameObject);
                }
            }
            else if (hit.collider.tag == "pistolAmmo")
            {
                GuiMessage = "Grab PISTOL AMMO";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Podniesiono pistol Ammo");
                    character.Ammo[0] += 10;
                    Destroy(hit.collider.gameObject);
                }
            }
            else if (hit.collider.tag == "Medikit")
            {
                GuiMessage = "Grab Medikit";
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("Podniesiono Medikit");
                    character.Health += 50;
                    Destroy(hit.collider.gameObject);
                }
            }
            else
            {
                GuiMessage = "";
            }
        }
        else
        {
            GuiMessage = "";
        }
    }
    void ChangeCamera()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TPCamera.depth = 0;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            TPCamera.depth = 1;
        }
    }
    public void OnGUI()
    {
        GUI.Button(new Rect(50, Screen.height-30, 50, 30), character.Ammo[character.SelectedGun].ToString()); // AMMO
        GUI.Button(new Rect(120, Screen.height - 30, 50, 30), character.Health.ToString()); // HEALTH

        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 280, 20), GuiMessage); // MESSAGE
        GUI.Box(new Rect(Screen.width / 2, Screen.height / 2, 10, 10), ""); // CROSSHAIR

    }
}
