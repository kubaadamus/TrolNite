using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    CharacterController ctrl;
    Vector3 movement = Vector3.zero;
    Character character;                //Pobiera główny element - character - żeby sie z nim komunikować

    //CharacterAudio//
    public AudioClip JumpAudioClip;
    public AudioClip FallDamageAudioClip;
    public AudioClip WalkAudioClip;
    public AudioClip GunPickUpAudioClip;
    public Vector3 LastPlayerPosition;
    float StepDistance = 5.0f;

    public float jumpFactor = 8.0f;
    public float speed = 8.5f;
    public float pushPower = 12.0f;
    public float fallDamage_MinMagnitude = 30.0f;
    public float fallDamage_Factor = 0.5f; // HealthDecrease = FallMagnitude * Factor

    //MouseMovement//
    public bool MouseMovementEnabled = true;
    public float mouseSensitivity = 100.0f;
    public float clampAngle = 80.0f;
    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axisn;
    public Camera TPCamera;
    public Camera FPCamera;
    //HandMovement//
    public GameObject Eyepos;                           //Pozycja broni przy oku
    public GameObject Hippos;                           //Pozycja broni przy biodrze
    public GameObject CurrentItemPosition;              //Aktualna pozycja tego co trzymam w ręce
    public GameObject BackItemPosition;                 //Pozycja itemu w plecaku
    public GameObject CurrentItemPositionDestination;
    GameObject CurrentItem;                             //Co trzymam w ręce
    bool DisplayCrosshair = true;
    int DestinationFieldOfView = 60;
    //InteractionRaycast//
    float InteractionDistance = 15;
    string GuiMessage = "";
    RaycastHit hit;
    Ray landingRay;
    public Texture CrossHairTexture;
    //==================//
    void Start()
    {
        ctrl = GetComponent<CharacterController>();
        character = GetComponent<Character>();
        LastPlayerPosition = transform.position;
        CurrentItemPositionDestination.transform.position = Hippos.transform.position;
    }
    void Update()
    {
        KeyboardMovement();                 //Obsługa ruchu klawiatury
        if(MouseMovementEnabled)
        {
            MouseMovement();                    //Obsługa ruchu myszy
        }

        HandMovement();                     //Obsługa ruchu rąk
        ChangeCamera();                     //Zmiana kamer
        CharacterInteractionRaycast();      //Obsługa interakcji użytkownika ze środowiskiem

    }
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;                                    //Pobranie rigidbody z ciała uderzanego
        if (!ctrl.isGrounded)                                                               //FallDamage
        {
            if (ctrl.velocity.magnitude > fallDamage_MinMagnitude)
            {
                character.Health -= (int)(ctrl.velocity.magnitude * fallDamage_Factor);
                Debug.Log("Walnales w ziemie z sila: " + ctrl.velocity.magnitude);
                AudioSourceHandlerScript.PlayAudio(FallDamageAudioClip, transform.position, 1.0f);
                character.HealthUiText.text = character.Health.ToString();
            }
        }

        if (body == null || body.isKinematic || hit.moveDirection.y < -0.3f)                //Odepchięcie ciała uderzonego
        {

        }
        else
        {
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, 0f);
            body.velocity = pushDir * pushPower;
        }
    }   //FALL DAMAGE 
    public void MouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        rotY += mouseX * mouseSensitivity * Time.deltaTime;
        rotX += mouseY * mouseSensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = Quaternion.Euler(character.transform.rotation.x, rotY, character.transform.rotation.z);
        FPCamera.transform.rotation = Quaternion.Euler(rotX, rotY, character.transform.rotation.z);
    }
    public void KeyboardMovement()
    {
        if (!ctrl.isGrounded) { movement.y += Physics.gravity.y * Time.deltaTime; }            // Jeśli gracz nie stoi na ziemi to grawitacja działa.
        if (Input.GetKeyDown(KeyCode.Space) && ctrl.isGrounded)                                //Skakanie
        {
            movement.y = jumpFactor;
            AudioSourceHandlerScript.PlayAudio(JumpAudioClip, transform.position, 1.0f);
        }
        if (Input.GetKeyDown(KeyCode.LeftShift)) { speed = 20.0f; }                            //Bieganie ON
        if (Input.GetKeyUp(KeyCode.LeftShift)) { speed = 8.5f; }                               //Bieganie OFF
        if (ctrl.isGrounded)                                                                //Pobieranie inputu
        {
            movement.z = Input.GetAxis("Vertical") * speed * transform.forward.z - Input.GetAxis("Horizontal") * speed * transform.forward.x;
            movement.x = Input.GetAxis("Vertical") * speed * transform.forward.x + Input.GetAxis("Horizontal") * speed * transform.forward.z;
        }
        ctrl.Move(movement * Time.deltaTime);

        if(ctrl.isGrounded && Vector3.Distance(transform.position, LastPlayerPosition)>StepDistance)
        {
            AudioSourceHandlerScript.PlayAudio(WalkAudioClip, transform.position, Random.Range(0.8f,1.2f),Random.Range(-0.5f,0.5f));
            LastPlayerPosition = transform.position;
        }
    }
    public void HandMovement()
    {
        if (Input.GetMouseButtonDown(1))
        {
            CurrentItemPositionDestination.transform.position = Eyepos.transform.position;
            DestinationFieldOfView = 40;
            DisplayCrosshair = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            CurrentItemPositionDestination.transform.position = Hippos.transform.position;
            DestinationFieldOfView = 60;
            DisplayCrosshair = true;
        }

        if (FPCamera.fieldOfView != DestinationFieldOfView)
        {
            FPCamera.fieldOfView += (DestinationFieldOfView - FPCamera.fieldOfView) / 20.0f;
        }
        if(CurrentItemPosition.transform.position != CurrentItemPositionDestination.transform.position)
        {
            CurrentItemPosition.transform.position += (CurrentItemPositionDestination.transform.position - CurrentItemPosition.transform.position)/5.0f;
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
    public void CharacterInteractionRaycast()
    {
        //Aktualizacja nowego raya
        landingRay.origin = FPCamera.transform.position;
        landingRay.direction = FPCamera.transform.forward;
        Debug.DrawRay(FPCamera.transform.position, FPCamera.transform.forward * InteractionDistance);

        //STRZELANIE RAYCASTEM
        if (Physics.Raycast(landingRay, out hit, InteractionDistance))
        {
            if (hit.collider.GetComponent<_gripable>())
            {
                GuiMessage = "Grab gripable " + hit.collider.name.ToString();
                if (Input.GetKey(KeyCode.F) && !CurrentItemPosition.GetComponent<SpringJoint>())
                {
                    if (hit.collider.tag == "NPC")
                    {
                        hit.collider.gameObject.GetComponent<npcMove>().DestroyNavMesh();
                    }
                    SpringJoint Joint = CurrentItemPosition.AddComponent(typeof(SpringJoint)) as SpringJoint;

                    Joint.anchor = new Vector3(0, 0, 0);
                    Joint.autoConfigureConnectedAnchor = false;
                    Joint.connectedAnchor = new Vector3(0, 0, 0);
                    Joint.minDistance = 0.0f;
                    Joint.maxDistance = 0.1f;
                    Joint.spring = 200;
                    Joint.damper = 0;
                    Joint.connectedBody = hit.rigidbody;
                    hit.rigidbody.drag = 4;
                    Joint.enableCollision = true;
                }
            }
            //ŁAPANIE BRONI
            else if (hit.collider.GetComponent<Gun>() && hit.collider.GetComponent<Gun>().Type != GunType.meelee)
            {
                GuiMessage = "Grab gun" + hit.collider.GetComponent<Gun>().Type;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (!character.GunsList.Exists(f => f.GetComponent<Gun>().Type == hit.collider.GetComponent<Gun>().Type))     //Czy na liscie broni znajduje sie .. szotgan ?
                    {
                        character.GunsList.Add(hit.collider.gameObject);                                                            //Wez do tej listy cały gameobject
                        hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                        hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                        Debug.Log("Podniesiono " + hit.collider.GetComponent<Gun>().Type + " health:" + hit.collider.GetComponent<Gun>().Health + " ammo: " + hit.collider.GetComponent<Gun>().AmmoLoaded);
                        hit.collider.gameObject.transform.position = BackItemPosition.transform.position;
                        hit.collider.gameObject.transform.rotation = BackItemPosition.transform.rotation;
                        hit.collider.gameObject.transform.SetParent(BackItemPosition.transform);
                        AudioSourceHandlerScript.PlayAudio(GunPickUpAudioClip, transform.position, 1.0f);
                    }
                    else
                    {
                        Debug.Log("Masz juz " + hit.collider.GetComponent<Gun>().Type + " podniesiono ammo: " + hit.collider.GetComponent<Gun>().AmmoLoaded + "sztuk");
                        Destroy(hit.collider.gameObject);
                    }

                }
            }
            //ŁAPANIE AMMO
            else if (hit.collider.GetComponent<GunAmmo>())
            {
                GuiMessage = "Grab AMMO! " + hit.collider.GetComponent<GunAmmo>().ammoType;
                if (Input.GetKeyDown(KeyCode.F))
                {

                    character.GunAmmoList.Add(hit.collider.gameObject);                                                            //Wez do tej listy cały gameobject
                    hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    hit.collider.gameObject.GetComponent<Rigidbody>().useGravity = false;
                    Debug.Log("Podniesiono " + hit.collider.GetComponent<GunAmmo>().ammoType + " amount: " + hit.collider.GetComponent<GunAmmo>().ammoAmount);
                    hit.collider.gameObject.transform.position = BackItemPosition.transform.position;
                    hit.collider.gameObject.transform.rotation = BackItemPosition.transform.rotation;
                    hit.collider.gameObject.transform.SetParent(BackItemPosition.transform);
                    AudioSourceHandlerScript.PlayAudio(GunPickUpAudioClip, transform.position, 1.0f);
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
        if (Input.GetKeyUp(KeyCode.F) && CurrentItemPosition.GetComponent<SpringJoint>())
        {
            CurrentItemPosition.GetComponent<SpringJoint>().connectedBody.GetComponent<Rigidbody>().drag = 0.2f;
            Destroy(CurrentItemPosition.GetComponent<SpringJoint>());

        }
    }
    public void OnGUI()
    {
        GUI.Button(new Rect(120, Screen.height - 30, 50, 30), character.Health.ToString()); // HEALTH

        GUI.Label(new Rect(Screen.width / 2, Screen.height / 2, 280, 20), GuiMessage); // MESSAGE

        if(DisplayCrosshair)
        {
            GUI.DrawTexture(new Rect((Screen.width / 2)-2,(Screen.height / 2)-2, 4, 4), CrossHairTexture);
        }

    }

}
