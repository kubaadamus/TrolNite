using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public static List<GameObject> NPCList = new List<GameObject>();
   	void Start () {
        foreach (GameObject fooObj in GameObject.FindGameObjectsWithTag("NPC"))
        {
            NPCList.Add(fooObj);
        }

        //Debug.Log("Ilosc NPC: " + NPCList.Count);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void KillNPC(GameObject KilledNPC)
    {
        NPCList.Remove(KilledNPC);
        //Debug.Log("Ilosc NPC: " + NPCList.Count);

        if(NPCList.Count==0)
        {
            SceneManager.LoadScene("S1");
        }
    }
}
