using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour {

    float[] times;
    Quaternion[] angs;
    float mult = 1.0f;

	// Use this for initialization
	void Start () {
        times = new float[2];
        times[0] = Random.value * mult;
        times[1] = Random.value * mult;

        angs = new Quaternion[2];
        angs[0] = Random.rotation;
        angs[1] = Random.rotation;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Test ()
    {

    }

    public void Evolve ()
    {

    }

    public void Reset ()
    {

    }
}
