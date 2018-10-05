using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceHandlerScript : MonoBehaviour
{

    public static GameObject SrcObject;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    public static void PlayAudio(AudioClip Clip, Vector3 Position, float Pitch, float Pan = 0)
    {
        SrcObject = new GameObject();
        (SrcObject.AddComponent<AudioSource>()).clip = Clip;
        SrcObject.GetComponent<AudioSource>().pitch = Pitch;
        SrcObject.GetComponent<AudioSource>().panStereo = Pan;
        SrcObject.transform.position = Position;
        SrcObject.GetComponent<AudioSource>().Play();
        SrcObject.AddComponent<AudioSourceObjectDestroyerScript>();
    }
}
