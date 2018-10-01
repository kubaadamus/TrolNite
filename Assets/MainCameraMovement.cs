using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainCameraMovement : MonoBehaviour {
	void Start () {
        RenderSettings.fog = false;
	}
	void Update () {

	}
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 80, 80), "HelloWorldxD");
    }
}
