using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript : MonoBehaviour {


    public string name;
    public GameObject Target;
    public EnemyPlantScriptHANDMADE skrypt;
	void Start () {
        skrypt = Target.GetComponent<EnemyPlantScriptHANDMADE>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter(Collider other)
    {
        skrypt.DamageHandler(name);
    }
}
