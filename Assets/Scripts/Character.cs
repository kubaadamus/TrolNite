using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunType { pistol, shotgun };

public class Character : MonoBehaviour
{
    public GameObject Particles;
    public GameObject BulletHitAudioSource;
    public string NazwaGracza = "";
    public bullet pocisk;
    public List<GunType> GunsList;
    public AudioClip Shot;
    public AudioClip OutOfAmmo;
    
    public int Health=50;
    public gun Gun;
    public int GunsCount = 1;
    public int SelectedGun = 0;
    public int[] Ammo = new int[2]; // Pistol ammo, shotgun ammo
    float speed = 8.5f;
    public AudioSource DesertShotAudioSource;
    public AudioSource DesertNoAmmoAudioSource;
    float jumpSpeed = 8.0f;
    CharacterController ctrl;
    Vector3 movement = Vector3.zero;
    float pushPower = 12.0f;
    Vector3 BoolDirection = new Vector3(0, 0, 0);
    bool machinegunIsFireing = false;
    RaycastHit hit;
    int ForDistance = 1000;
    int BackDistance = 5;
    public GameObject RayCastPinpointObject;

    float LastTimeBulletWasShot = 0;
    
    void Start()
    {
        LastTimeBulletWasShot = Time.timeSinceLevelLoad;
        Cursor.lockState = CursorLockMode.Locked;
        Ammo[0] = 1000;
        GunsList.Add(GunType.pistol);
        ctrl = GetComponent<CharacterController>();
    }

    void Update()
    {

        //SHOOT
        if (Input.GetMouseButtonDown(0) && !machinegunIsFireing)
        {
            if(Ammo[SelectedGun]>0)
            {
                

                machinegunIsFireing = true;

            }
            else
            {

            }

        }

        if(machinegunIsFireing && Time.timeSinceLevelLoad>LastTimeBulletWasShot+0.05f)
        {
            LastTimeBulletWasShot = Time.timeSinceLevelLoad + 0.05f;
            if (Ammo[SelectedGun] > 0)
            {

                Destroy(Instantiate(pocisk, Gun.Barrel.transform.position, Gun.Barrel.transform.rotation), 1);
                Ammo[SelectedGun]--;
                //Debug.Log("Gracz: " + pocisk.NazwaGracza + " wystrzelił pocisk i ma teraz " + Ammo[SelectedGun] + " ammo.");
                GunShoot();
                DesertShotAudioSource.Play();
                machinegunIsFireing = true;

            }
            else
            {
                DesertNoAmmoAudioSource.Play();
            }
            BulletImpactDetection();
        }





        if (Input.GetMouseButtonUp(0)){
            machinegunIsFireing = false;
        }
        //=============//
        GunSelect();
        if (!ctrl.isGrounded)
        {
            movement.y += Physics.gravity.y * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && ctrl.isGrounded)
        {
            movement.y = jumpSpeed;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 20.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 8.5f;
        }

        if (ctrl.isGrounded)
        {
            movement.z = Input.GetAxis("Vertical") * speed * transform.forward.z - Input.GetAxis("Horizontal") * speed * transform.forward.x;
            movement.x = Input.GetAxis("Vertical") * speed * transform.forward.x + Input.GetAxis("Horizontal") * speed * transform.forward.z;
        }

        ctrl.Move(movement * Time.deltaTime);
    }
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {

        if (!ctrl.isGrounded)
        {
            //Debug.Log("Walnales w ziemie z sila: " + ctrl.velocity.magnitude);
            if (ctrl.velocity.magnitude > 22)
            {
                Health -= 50;
            }
        }


        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            return;

        }
        if (hit.moveDirection.y < -0.3f)
        {
            return;
        }
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, 0f);
        body.velocity = pushDir * pushPower;

        //FALL DAMAGE


    }
    public void GunSelect()
    {

        if (Input.GetAxis("Mouse ScrollWheel")>0)
        {
            if(SelectedGun<GunsCount-1)
            {
                SelectedGun ++;
                //Debug.Log("Wybrano: " + GunsList[SelectedGun]);
                ChangeGunMesh();

            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(SelectedGun>0)
            {
                SelectedGun--;
                //Debug.Log("Wybrano: " + GunsList[SelectedGun]);
                ChangeGunMesh();
            }
        }
    }
    public void ChangeGunMesh()
    {
        if (GunsList[SelectedGun] == GunType.pistol)
        {
            Gun.GunMesh.mesh = Gun.pistolMesh;
        }
        if (GunsList[SelectedGun] == GunType.shotgun)
        {
            Gun.GunMesh.mesh = Gun.shotgunMesh;
        }
    }
    public void GunShoot()
    {
        Gun.GetComponent<Animation>().Stop();
        Gun.GetComponent<Animation>().Play("Desert_Shoot");

        

    }
    void BulletImpactDetection()
    {
        //WYKRYWANIE UDERZENIA

        Ray landingRayForward = new Ray(Gun.Barrel.transform.position, Gun.Barrel.transform.forward);
        Debug.DrawRay(Gun.Barrel.transform.position, Gun.Barrel.transform.forward * ForDistance);
        //STRZELANIE RAYCASTEM
        if (Physics.Raycast(landingRayForward, out hit, ForDistance))
        {

            // Find the line from the gun to the point that was clicked.
            Vector3 incomingVec = hit.point - Gun.Barrel.transform.position;

            // Use the point's normal to calculate the reflection vector.
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

            Debug.DrawRay(hit.point, reflectVec);

            Instantiate(Particles, hit.point, Quaternion.LookRotation(reflectVec));

            GameObject NewRayCastPinpoint = Instantiate(RayCastPinpointObject, hit.point +hit.normal.normalized/50.0f, Quaternion.LookRotation(hit.normal));



            NewRayCastPinpoint.transform.SetParent(hit.collider.transform);



            if (hit.collider.tag == "NPC")
            {
                hit.collider.gameObject.GetComponent<npcMove>().DestroyNavMesh();
            }

            try
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(landingRayForward.direction * 200);
            }
            catch (System.Exception) { }

            try
            {
                hit.collider.gameObject.GetComponent<ColliderScript>().skrypt.DamageHandler(hit.collider.gameObject.GetComponent<ColliderScript>().name, "bullet");

            }
            catch (System.Exception) { }

            Instantiate(BulletHitAudioSource, hit.point, transform.rotation);


        }

    }
}
