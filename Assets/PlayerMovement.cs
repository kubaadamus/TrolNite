using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {


    //DANE OBIEKTU
    public static float cena;
    public static string nazwa;
    public static float wysokosc;
    public static float szerokosc;
    public static float glebokosc;
    public static Sprite obrazek;
    public GameObject camera;

    Rigidbody CamBody;
    CapsuleCollider CamCollider;
    bool InAir = false;
    float brakeFactor = 10.0f; //Chamowanie gracza
    float acceleration = 25.0f; //Przyspieszenie graczas


    //STEROWANIE
    bool MouseLook = true;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    //RAYCAST
    float Distance = 10;

    void Start()
    {
        //Sterowanie
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
        Cursor.visible = false;
        CamBody = GetComponent<Rigidbody>();
        CamCollider = GetComponent<CapsuleCollider>();
    }

    void Update()
    {

        //Sterowanie
        if (MouseLook)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = -Input.GetAxis("Mouse Y");
            float PrzodTyl = Input.GetAxis("Vertical");
            float LewoPrawo = Input.GetAxis("Horizontal");
            float Jump = Input.GetAxis("Jump");

            var locVel = transform.InverseTransformDirection(CamBody.velocity);
            if (!InAir)
            {
                if (PrzodTyl == 0) { CamBody.AddRelativeForce(0, 0,-locVel.z*brakeFactor); }
                if (LewoPrawo == 0) { CamBody.AddRelativeForce(-locVel.x*brakeFactor, 0, 0); }
                CamBody.AddRelativeForce(new Vector3(LewoPrawo * acceleration, 0, PrzodTyl * acceleration));
            }
            if (Input.GetKeyDown(KeyCode.Space) && !InAir)
            {
                CamBody.AddForce(0, 5, 0, ForceMode.Impulse);
                InAir = true;
                CamBody.drag = 0.0f;
            }
            rotY += mouseX * mouseSensitivity * Time.deltaTime;
            rotX += mouseY * mouseSensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

            Quaternion localRotation = Quaternion.Euler(0.0f, rotY, 0.0f);
            transform.rotation = localRotation;
            Quaternion localRotation2 = Quaternion.Euler(rotX, rotY, 0.0f);
            camera.transform.rotation = localRotation2;

        }
        Debug.DrawRay(transform.position, transform.forward * Distance);

        //Raycast
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(landingRay, out hit, Distance))
        {
            if (hit.collider.tag == "target" && Input.GetMouseButtonDown(0))
            {
            }
            if (hit.collider.tag == "")
            {
            }
        }
    } 
    void OnCollisionEnter(Collision col)
    {


        if (col.gameObject.tag == "floor")
        {
            Debug.Log(col.gameObject.tag);
            InAir = false;
            Debug.Log(InAir);
            CamBody.drag = 1f;
        }
    }
}
