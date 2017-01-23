using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Toggle : MonoBehaviour {
    bool MainCam = true;
    public Camera Main;
    public Camera CloseCam;
    // Use this for initialization
	void Start () {
       // Main = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("space"))
        {
            MainCam = !MainCam;
        }
        Main.enabled = MainCam;
        CloseCam.enabled = !MainCam;

	}
}
