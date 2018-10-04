using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_multiplayer_Script : MonoBehaviour {

    void OnCollisionEnter(Collision collision)
    {
        var hit = collision.gameObject;
        var health = hit.GetComponent<HealthScript>();
        if (health != null)
        {
            health.TakeDamage(10);
        }

        Destroy(gameObject);
    }
}
