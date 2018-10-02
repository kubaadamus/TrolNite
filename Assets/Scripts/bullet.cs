using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour {

    public GameObject Particles;
    public string NazwaGracza = "";
    bool accelarate = true;
    Rigidbody BulletBody;
    void Start () {
        BulletBody = GetComponent<Rigidbody>();
        if (accelarate)
        {
            BulletBody.AddRelativeForce(0, 0, 100f, ForceMode.Impulse);
            accelarate = false;
        }
        Destroy(this.gameObject, 3);
    }
	
	// Update is called once per frame
	void Update () {


    }

    private void OnCollisionEnter(Collision collision)
    {

        Instantiate(Particles, transform.position,transform.rotation);
        Destroy(gameObject);
    }

}
