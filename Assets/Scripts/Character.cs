﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum GunType { pistol, shotgun };

public class Character : MonoBehaviour
{
    public string NazwaGracza = "";
    public bullet pocisk;
    public List<GunType> GunsList;
    public int Health=50;
    public gun Gun;
    public int GunsCount = 1;
    public int SelectedGun = 0;
    public int[] Ammo = new int[2]; // Pistol ammo, shotgun ammo
    float speed = 5.5f;
    float jumpSpeed = 8.0f;
    CharacterController ctrl;
    Vector3 movement = Vector3.zero;
    float pushPower = 12.0f;
    Vector3 BoolDirection = new Vector3(0, 0, 0);
    void Start()
    {
        Ammo[0] = 10;
        GunsList.Add(GunType.pistol);
        ctrl = GetComponent<CharacterController>();
    }
    void Update()
    {

        //SHOOT
        if (Input.GetMouseButtonDown(0))
        {
            if(Ammo[SelectedGun]>0)
            {
                pocisk.NazwaGracza = NazwaGracza;
                Instantiate(pocisk, Gun.Barrel.transform.position, Gun.Barrel.transform.rotation);
                Ammo[SelectedGun]--;
                //Debug.Log("Gracz: " + pocisk.NazwaGracza + " wystrzelił pocisk i ma teraz " + Ammo[SelectedGun] + " ammo.");
            }

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
            speed = 10.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5.5f;
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
}
