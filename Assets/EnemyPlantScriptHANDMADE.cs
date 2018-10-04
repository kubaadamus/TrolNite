using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPlantScriptHANDMADE : MonoBehaviour {

    public enum AnimationState {Idle,Hit,Damage};
    public int health = 100;
    Animation anim;
   	void Start () {
        anim = GetComponent<Animation>();
        anim.Play(AnimationState.Hit.ToString());

	}
	
	// Update is called once per frame
	void Update () {

        if (!anim.isPlaying)
        {
            anim.Play("Idle");
        }
		
	}
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PLANT AUA XD");
        anim.Stop();
        anim.Play("Damage");

    }
    public void DamageHandler(string ColliderName, string other)
    {
        if(other=="bullet")
        {
            if (ColliderName == "headCollider")
            {
                Debug.Log("Obsługuję Headshota!");
                anim.Stop();
                anim.Play("Damage");
                health -= 50;
            }
            else if (ColliderName == "bodyCollider")
            {
                Debug.Log("Obsługuję Body Shota ! ");
                anim.Stop();
                anim.Play("Damage");
                health -= 20;
            }

            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
