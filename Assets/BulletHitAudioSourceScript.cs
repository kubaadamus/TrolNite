using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitAudioSourceScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().pitch = (Random.Range(0.5f, 1.5f));
        GetComponent<AudioSource>().Play();

        Destroy(gameObject, 3.0f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
