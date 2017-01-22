﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evolution : MonoBehaviour {

    Creature[] creatures;
    Component[] stuff;
    float time_since_last_evolution;
    public int generation = 0;
    public float bestDistance = 0.0f;
    public float worstDistance = 0.0f;
    public float average = 0.0f;
    public LineGraphManager LGM;

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
            //LGM.UpdateValues(bestDistance, worstDistance);
            //LGM.ShowGraph();
            generation++;
            Evolve();
            time_since_last_evolution = Time.time;
        }
	}

    void Evolve()
    {
        string kilt = "Killed: ";
        bestDistance = 0.0f;
        worstDistance = float.MaxValue;
        float totalDistance = 0;
        for (int i = 0; i < creatures.Length; i++)
        {
            totalDistance += creatures[i].GetDistanceTraveled();
            if (creatures[i].GetDistanceTraveled() > bestDistance) bestDistance = creatures[i].GetDistanceTraveled();
            if (creatures[i].GetDistanceTraveled() < worstDistance) worstDistance = creatures[i].GetDistanceTraveled();
        }
        average = totalDistance / creatures.Length;
        for (int i = 0; i < creatures.Length; i++)
        {
            creatures[i].survivalChance = Mathf.Clamp((creatures[i].GetDistanceTraveled()-average)/average, -1.0f, 1.0f);
            creatures[i].survivalChance = (creatures[i].survivalChance + 1) / 2;
            if (creatures[i].survivalChance < Random.value)
            {
                kilt = kilt + i + ", ";
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
                    float rando = Random.value;
                    if (rando < 0.001f)
                    {
                        myMvs[j] = new Move();
                        //print("Mutation! " + i + " has gotten a random move!");
                    }
                    else if(rando > 0.1f && rando < 0.3f)
                    {
                        myMvs[j] = Move.RandAvg(mvsMom[j], mvsDad[j]);
                    }
                    else
                    {
                        myMvs[j] = Move.Avg(mvsMom[j], mvsDad[j]);
                    }
                    if (rando < 0.002f && rando > 0.004f)
                    {
                        myMvs[j].time += Random.value - 0.5f;
                        //print("Mutation! " + i + " has gotten a move's time adjusted!");
                    }
                    else if (rando < 0.004f && rando > 0.006f)
                    {
                        myMvs[j].ang = Quaternion.Slerp(myMvs[j].ang, Random.rotation, Random.value);
                        //print("Mutation! " + i + " has gotten a move's angle adjusted!");
                    }
                }
                creatures[i].ChangeLegs(myMvs);
            }
        }
        for (int i = 0; i < creatures.Length; i++)
        {
            creatures[i].Reset();
        }
        //print(kilt);
    }

    int findNotDead()
    {
        int i = Mathf.RoundToInt(Random.value * (creatures.Length-1));
        while (creatures[i].dead && creatures[i].survivalChance < Random.value)
        {
            i = Mathf.RoundToInt(Random.value * (creatures.Length-1));
        }
        return i;
    }
}