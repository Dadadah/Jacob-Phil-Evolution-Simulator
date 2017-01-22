using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour {

    Creature[] creatures;

	// Use this for initialization
	void Start () {
        creatures = gameObject.GetComponentsInChildren(typeof(Creature)) as Creature[];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
