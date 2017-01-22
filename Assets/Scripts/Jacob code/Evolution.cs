using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour {

    Creature[] creatures;
    Component[] stuff;
    float time_since_last_evolution;
    int generation = 0;

	// Use this for initialization
	void Start () {
        time_since_last_evolution = Time.time;
        stuff  = gameObject.GetComponentsInChildren(typeof(Creature), true);
        creatures = new Creature[stuff.Length];
        for (int i = 0; i < stuff.Length; i++)
        {
            creatures[i] = stuff[i] as Creature;
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - time_since_last_evolution > 20)
        {
            print("Evolving!");
            generation++;
            Evolve();
            time_since_last_evolution = Time.time;
        }
	}

    void Evolve()
    {
        float totalDistance = 0;
        for (int i = 0; i < creatures.Length; i++)
        {
            totalDistance += creatures[i].GetDistanceTraveled();
        }
        float average = totalDistance / creatures.Length;
        print("Average Distance: " + average + ", Generation: " + generation);
        for (int i = 0; i < creatures.Length; i++)
        {
            float survivalchance = Random.value;
            if (survivalchance < 0.02 || (survivalchance < 0.98 && creatures[i].GetDistanceTraveled() < average))
            {
                creatures[i].dead = true;
            }
        }
        for (int i = 0; i < creatures.Length; i++)
        {
            if (creatures[i].dead)
            {
                Creature Mom = creatures[findNotDead()];
                Creature Dad = creatures[findNotDead()];
                Move[] mvsMom = Mom.WatRMyLegs();
                Move[] mvsDad = Dad.WatRMyLegs();
                Move[] myMvs = new Move[mvsMom.Length];
                for (int j = 0; j < myMvs.Length; j++)
                {
                    myMvs[j] = Move.Avg(mvsMom[j], mvsDad[j]);
                }
                creatures[i].ChangeLegs(myMvs);
            }
        }
        for (int i = 0; i < creatures.Length; i++)
        {
            creatures[i].Reset();
        }
    }

    int findNotDead()
    {
        int i = Mathf.RoundToInt(Random.value * (creatures.Length-1));
        while (creatures[i].dead)
        {
            i = Mathf.RoundToInt(Random.value * (creatures.Length-1));
        }
        return i;
    }
}
