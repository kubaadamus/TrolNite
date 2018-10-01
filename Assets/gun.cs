using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour {

    public Mesh pistolMesh;
    public Mesh rifleMesh;
    public MeshFilter GunMesh;
    public GameObject Barrel;
    public GameObject Bullet;
    public GameObject Eyepos;
    public GameObject Hippos;
	void Start () {
        GunMesh = GetComponent<MeshFilter>();
        GunMesh.mesh = pistolMesh;
    }
	
	void Update () {

        if(Input.GetMouseButtonDown(1))
        {
            transform.position = Eyepos.transform.position;
        }
        if (Input.GetMouseButtonUp(1))
        {
            transform.position = Hippos.transform.position;
        }
	}
}
