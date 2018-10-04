using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour {

    public Mesh pistolMesh;
    public Mesh shotgunMesh;
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

	}
}
