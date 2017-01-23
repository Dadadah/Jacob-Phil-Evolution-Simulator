using UnityEngine;
using System.Collections;

public class Spin : MonoBehaviour {
    public Vector3 Speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localEulerAngles += Speed;
	}
}
