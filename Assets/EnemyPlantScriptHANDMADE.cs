using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlantScriptHANDMADE : MonoBehaviour {

    public enum AnimationState {Idle,Hit,Damage};
    public CapsuleCollider[] CapsuleCollidersArray;
    public string nameEnemyPlanta = "";
    Animation anim;
   	void Start () {
        anim = GetComponent<Animation>();
        anim.Play(AnimationState.Hit.ToString());

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.G))
        {
            anim.Play("Hit");


        }

        if (!anim.isPlaying)
        {
            anim.Play("Idle");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            anim.Stop();
            anim.Play("Damage");
        }
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PLANT AUA XD");
        anim.Stop();
        anim.Play("Damage");

    }
    public void DamageHandler(string ColliderName)
    {
        if(ColliderName=="headCollider")
        {
            Debug.Log("Obsługuję Headshota!");
        }
        else if (ColliderName == "bodyCollider")
        {
            Debug.Log("Obsługuję Body Shota ! ");
        }
    }
}
