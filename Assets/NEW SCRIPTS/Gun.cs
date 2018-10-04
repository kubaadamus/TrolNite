using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunType {meelee, pistol, shotgun, rifle, sniper, granade, mine };
public class GunItem // Gun Item specjalnie do przechowywania w ekwipunku
{
    public int GunHealth = 100;
    public int GunAmmoLoaded = 10;
    public GunType Type;
    public GunItem(GunType _gunType, int _gunHealth)
    {
        Type = _gunType;
        GunHealth = _gunHealth;
    }
}

public class Gun : MonoBehaviour {  //Rzeczywisty obiekt, który będzie obsługiwany w grze
    //Wszystko to idzie do guna!
    public GameObject Particles;
    public GameObject BulletHitAudioSource;
    public bullet pocisk;
    public AudioClip ShotAudioClip;
    public AudioClip OutOfAmmoAudioClip;
    public AudioSource DesertShotAudioSource;
    public AudioSource DesertNoAmmoAudioSource;
    bool machinegunIsFireing = false;
    RaycastHit hit;
    int ForDistance = 1000;
    int BackDistance = 5;
    public GameObject RayCastPinpointObject;
    float LastTimeBulletWasShot = 0;
    public GameObject GunBarrelPosition;
    //-----wszystko powyzej do guna!
    public GunType Type;
    public int Health = 100;
    public GunAmmoType AmmoType;
    public int AmmoLoaded = 10;

    public void Shot()
    {
        Debug.Log("Strzeliles z :" + Type);
    }

    /*
    void Start()
    {
        LastTimeBulletWasShot = Time.timeSinceLevelLoad;
    }



    void Update()
    {
        //SHOOT
        if (Input.GetMouseButtonDown(0) && !machinegunIsFireing)
        {
            if (AmmoLoaded > 0)
            {


                machinegunIsFireing = true;

            }
            else
            {

            }

        }

        if (machinegunIsFireing && Time.timeSinceLevelLoad > LastTimeBulletWasShot + 0.05f)
        {
            LastTimeBulletWasShot = Time.timeSinceLevelLoad + 0.05f;
            if (AmmoLoaded > 0)
            {

                Destroy(Instantiate(pocisk, GunBarrelPosition.transform.position, GunBarrelPosition.transform.rotation), 1);
                AmmoLoaded--;
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
    }

    public void GunShoot()
    {
        GetComponent<Animation>().Stop();
        GetComponent<Animation>().Play("Shot");



    }
    void BulletImpactDetection()
    {
        //WYKRYWANIE UDERZENIA

        Ray landingRayForward = new Ray(GunBarrelPosition.transform.position, GunBarrelPosition.transform.forward);
        Debug.DrawRay(GunBarrelPosition.transform.position, GunBarrelPosition.transform.forward * ForDistance);
        //STRZELANIE RAYCASTEM
        if (Physics.Raycast(landingRayForward, out hit, ForDistance))
        {

            // Find the line from the gun to the point that was clicked.
            Vector3 incomingVec = hit.point - GunBarrelPosition.transform.position;

            // Use the point's normal to calculate the reflection vector.
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

            Debug.DrawRay(hit.point, reflectVec);

            Instantiate(Particles, hit.point, Quaternion.LookRotation(reflectVec));

            GameObject NewRayCastPinpoint = Instantiate(RayCastPinpointObject, hit.point + hit.normal.normalized / 50.0f, Quaternion.LookRotation(hit.normal));



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
    */
}
