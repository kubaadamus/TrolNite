using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    float speed = 5.5f;
    float jumpSpeed = 8.0f;
    CharacterController ctrl;
    Vector3 movement = Vector3.zero;
    float pushPower = 12.0f;

    Vector3 BoolDirection = new Vector3(0, 0, 0);

    void Start () {
        ctrl = GetComponent<CharacterController>();
	}
	
	void Update () {

        if (!ctrl.isGrounded)
        {
            movement.y += Physics.gravity.y * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space) && ctrl.isGrounded)
        {
            movement.y = jumpSpeed;
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = 10.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = 5.5f;
        }

        if (ctrl.isGrounded)
        {
            movement.z = Input.GetAxis("Vertical") * speed * transform.forward.z - Input.GetAxis("Horizontal")*speed*transform.forward.x;
            movement.x = Input.GetAxis("Vertical") * speed * transform.forward.x + Input.GetAxis("Horizontal") * speed * transform.forward.z;
        }

        ctrl.Move(movement * Time.deltaTime);
	}
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;
        if(body == null || body.isKinematic)
        {
            return;

        }
        if(hit.moveDirection.y < -0.3f)
        {
            return;
        }
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0f, 0f);
        body.velocity = pushDir * pushPower;
    }
    public void CheckDirection()
    {

        if (ctrl.velocity.x > 0)
        {
            BoolDirection.x = 1;
        }
        else if (ctrl.velocity.x == 0)
        {
            BoolDirection.x = 0;
        }
        else if (ctrl.velocity.x < 0)
        {
            BoolDirection.x = -1;
        }
        if (ctrl.velocity.z > 0)
        {
            BoolDirection.z = 1;
        }
        else if (ctrl.velocity.z == 0)
        {
            BoolDirection.z = 0;
        }
        else if (ctrl.velocity.z < 0)
        {
            BoolDirection.z = -1;
        }
    }
}
