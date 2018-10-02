using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour {

    ParticleSystem sys;
	void Start () {
        sys = GetComponent<ParticleSystem>();
        sys.Emit(20);
        Destroy(gameObject, 0.2f);
    }
	
	void Update () {

	}
}
