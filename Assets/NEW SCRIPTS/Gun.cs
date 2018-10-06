using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunType { meelee, pistol, shotgun, rifle, sniper, granade, mine };

public class Gun : MonoBehaviour
{  //Rzeczywisty obiekt, który będzie obsługiwany w grze
    //DANE GUNA//
    public GunType Type;
    public int Health = 100;
    public GunAmmoType AmmoType;
    public int AmmoLoaded = 10;
    public GameObject GunBarrelPosition;
    public int GunRange = 1000;
    public float GunDamage = 0;
    public int MaxAmmo = 0;
    //AUDIO GUNA//
    public AudioClip ShotClip;
    public AudioClip NoAmmoClip;
    public AudioClip BulletImpactClip;
    public AudioClip ReloadClip;
    public AudioClip GunDrop;
    //RAYCAST GUNA//
    RaycastHit hit;
    public Light RedDot;
    //EFEKT UDERZENIA GUNA//
    public GameObject GunParticles;
    public GameObject RayCastPinpointObject;
    public bullet pocisk;
    //ZMIENNE KARABINU MASZYNOWEGO
    bool machinegunIsFireing = false;
    float LastTimeBulletWasShot = 0;
    //ANIMACJE GUNA//
    Animation GunAnimation;

    void Start()
    {
        LastTimeBulletWasShot = Time.timeSinceLevelLoad;
        if(RedDot!=null)
        {
            RedDot = Instantiate(RedDot, transform.position, Quaternion.Euler(0, 0, 0));
        }

    }
    void Update()
    {
        RedDotHandler();
    }
    public void Shot()
    {
        if (AmmoLoaded > 0)
        {
            Debug.Log("Strzeliles z :" + Type);
            GetComponent<Animation>().Stop();
            GetComponent<Animation>().Play("Shoot");
            if(Type!=GunType.meelee)
            {
                AmmoLoaded -= 1;
            }
            AudioSourceHandlerScript.PlayAudio(ShotClip, transform.position, 1.0f);
            BulletImpactDetection();
        }
        else
        {
            Debug.Log("BRAK AMMO!");
            AudioSourceHandlerScript.PlayAudio(NoAmmoClip, transform.position, 1.0f);
        }
    }
    void RedDotHandler()
    {
        //WYKRYWANIE UDERZENIA
        if(GunBarrelPosition!=null)
        {
            Ray GunRayRedDot = new Ray(GunBarrelPosition.transform.position, GunBarrelPosition.transform.forward);
            Debug.DrawRay(GunBarrelPosition.transform.position, GunBarrelPosition.transform.forward * GunRange);
            //STRZELANIE RAYCASTEM
            if (Physics.Raycast(GunRayRedDot, out hit, GunRange))
            {
                RedDot.transform.position = hit.point + hit.normal.normalized / 50.0f;
                RedDot.transform.rotation = Quaternion.LookRotation(hit.normal);
            }
            else
            {
                RedDot.transform.position = new Vector3(0, 0, 0);
            }
        }

    }
    void BulletImpactDetection()
    {
        //WYKRYWANIE UDERZENIA
        Ray GunRay = new Ray(GunBarrelPosition.transform.position, GunBarrelPosition.transform.forward);
        Debug.DrawRay(GunBarrelPosition.transform.position, GunBarrelPosition.transform.forward * GunRange);
        //STRZELANIE RAYCASTEM
        if (Physics.Raycast(GunRay, out hit, GunRange))
        {

            // Find the line from the gun to the point that was clicked.
            Vector3 incomingVec = hit.point - GunBarrelPosition.transform.position;
            // Use the point's normal to calculate the reflection vector.
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);
            Debug.DrawRay(hit.point, reflectVec);
            Instantiate(GunParticles, hit.point, Quaternion.LookRotation(reflectVec));
            GameObject NewRayCastPinpoint = Instantiate(RayCastPinpointObject, hit.point + hit.normal.normalized / 50.0f, Quaternion.LookRotation(hit.normal));
            NewRayCastPinpoint.transform.SetParent(hit.collider.transform);

            if (hit.collider.tag == "NPC")
            {
                hit.collider.gameObject.GetComponent<npcMove>().DestroyNavMesh();
            }
            try
            {
                hit.collider.gameObject.GetComponent<Rigidbody>().AddForce(GunRay.direction * 200 * GunDamage);
            }
            catch (System.Exception) { }
            try
            {
                hit.collider.gameObject.GetComponent<ColliderScript>().skrypt.DamageHandler(hit.collider.gameObject.GetComponent<ColliderScript>().name, "bullet");
            }
            catch (System.Exception) { }
            AudioSourceHandlerScript.PlayAudio(BulletImpactClip, hit.point, Random.Range(0.5f, 1.5f));
        }
    }
    public void Reload(int amount)
    {
        AmmoLoaded += amount;
        AudioSourceHandlerScript.PlayAudio(ReloadClip, transform.position, 1.0f);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("GUN KOLIDUJE z siłą" + collision.relativeVelocity.magnitude);
        AudioSourceHandlerScript.PlayAudio(GunDrop, transform.position, Random.Range(0.8f, 1.2f), 0, collision.relativeVelocity.magnitude / 40.0f);
    }
}