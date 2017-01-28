using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Evolution : MonoBehaviour {

    Creature[] creatures;
    Creature[] oldCreatures;
    Component[] stuff;

    float time_since_last_evolution;
    bool FEV = false;
    bool extraHud = false;
    public int x = 10;
    public int y =10;
    public int spacing = 15;
    public int generation = 0;
    public int lastKilled = 0;
    public float bestDistance = 0.0f;
    public float worstDistance = 0.0f;
    public float average = 0.0f;
    public float standardDeviation = 0.0f;
    public GameObject wombat;
    GameObject bestWombat;
    public GameObject CloseCam;

	void Start () {
        Vector3 loc = new Vector3();
        loc = loc + (Vector3.right * spacing * (x / 2));
        loc = loc + (Vector3.forward * spacing * (y / 2));
        for (int i = 0; i < y; i++)
        {
            for (int j = 0; j < x; j++)
            {
                GameObject wom = Instantiate(wombat, gameObject.GetComponent(typeof(Transform)) as Transform) as GameObject;
                Transform tran = wom.GetComponent(typeof(Transform)) as Transform;
                Rigidbody rb = wom.GetComponentInChildren(typeof(Rigidbody)) as Rigidbody;
                rb.isKinematic = true;
                tran.position = loc;
                rb.isKinematic = false;
                loc = loc + (Vector3.left * spacing);
            }
            loc = loc + (Vector3.back * spacing) - (Vector3.left * spacing * x);
        }

        time_since_last_evolution = Time.time;
        stuff  = gameObject.GetComponentsInChildren(typeof(Creature), true);
        creatures = new Creature[stuff.Length];
        for (int i = 0; i < stuff.Length; i++)
        {
            creatures[i] = stuff[i] as Creature;
        }

        Physics.IgnoreLayerCollision(8, 8, true);

        CsvReadWrite.StartFile();
    }
	
	void Update () {
		if (Time.time - time_since_last_evolution > 60)
        {
            print("Evolving!");
            generation++;
            Evolve();
            time_since_last_evolution = Time.time;
            //LGM.UpdateValues(bestDistance, worstDistance);
            //LGM.ShowGraph();
            CsvReadWrite.Save(new string[] { generation.ToString(), worstDistance.ToString(), average.ToString(), bestDistance.ToString(), standardDeviation.ToString() });
        }
        if (Input.GetKeyDown("tab")) extraHud = !extraHud;
	}

    //Draws GUI on Screen
    #region DrawHud
    void OnGUI()
    {

        GUILayout.Label("Generation: "+generation.ToString());
        if (extraHud)
        {
            GUILayout.Label("Killed Last Gen: " + lastKilled);
            GUILayout.Label("");
            GUILayout.Label("Best Distance:  " + bestDistance.ToString("F3"));
            GUILayout.Label("Mean Distance:  " + average.ToString("F3"));
            GUILayout.Label("Worst Distance: " + worstDistance.ToString("F3"));
            GUILayout.Label("Standard Deviation: " + standardDeviation.ToString("F3"));
            FEV = GUILayout.Toggle(FEV, "Enable FEV");
            GUILayout.Label("");
            GUILayout.Label("Cam Speed: " + CloseCam.GetComponent<Spin>().Speed.y.ToString("F3"));
            CloseCam.GetComponent<Spin>().Speed.y = GUILayout.HorizontalSlider(CloseCam.GetComponent<Spin>().Speed.y, -1.0F, 1.0F);
            GUILayout.Label("");
            GUILayout.Label("Sim Speed: " + Time.timeScale.ToString("G2"));
            Time.timeScale = GUILayout.HorizontalSlider(Time.timeScale,0.25F, 20.0F);

        }
        else
        {
            GUILayout.Label("Killed Last Gen: " + lastKilled);
            GUILayout.Label("");
            GUILayout.Label("Mean Distance:  " + average.ToString("F3"));
            GUILayout.Label("Standard Deviation: " + standardDeviation.ToString("F3"));
            GUILayout.Label("");
            GUILayout.Label("Sim Speed: " + Time.timeScale.ToString("G2"));
        }



    }
    #endregion

    void Evolve()
    {
        lastKilled = 0;
        bestDistance = 0.0f;
        worstDistance = float.MaxValue;
        float[] totalDistance = new float[creatures.Length];
        creatures.OrderByDescending(Creature => Creature.GetDistanceTraveled());
        oldCreatures = creatures;

        //Test Fitness
        for (int i = 0; i < creatures.Length; i++)
        {
            totalDistance[i] = creatures[i].GetDistanceTraveled();
            if (creatures[i].GetDistanceTraveled() > bestDistance)
            {
                bestDistance = creatures[i].GetDistanceTraveled();
                bestWombat = creatures[i].gameObject;
            }
            if (creatures[i].GetDistanceTraveled() < worstDistance) worstDistance = creatures[i].GetDistanceTraveled();
        }

        //Statistics Calculations
        average = totalDistance.Average();
        standardDeviation = 0;
        for (int i = 0; i < totalDistance.Length;i++)
        {
            standardDeviation += Mathf.Pow(totalDistance[i] - average, 2);
        }
        standardDeviation /= totalDistance.Length;
        standardDeviation = Mathf.Sqrt(standardDeviation);

        //Death
        //i == 10 because we want to preserve the top 10, no matter what.
        for (int i = 10; i < creatures.Length; i++)
        {
            creatures[i].survivalChance = creatures[i].GetDistanceTraveled();
            if (creatures[i].survivalChance < average - standardDeviation)
            {
                creatures[i].survivalChance = 0.0f;
            }
            else if (creatures[i].survivalChance > average + standardDeviation)
            {
                creatures[i].survivalChance = 1.0f;
            }
            else
            {
                creatures[i].survivalChance = Mathf.Clamp((creatures[i].GetDistanceTraveled() - (2 * standardDeviation)) / (2 * standardDeviation), -1.0f, 1.0f);
                creatures[i].survivalChance = (creatures[i].survivalChance + 1) / 2;
            }
            if (creatures[i].survivalChance < Random.value || (creatures[i].GetDistanceTraveled() < average && FEV))
            {
                creatures[i].dead = true;
                lastKilled++;
            }
        }

        //Reproduction
        for (int i = 0; i < creatures.Length; i++)
        {
            if (i >= 10)
            {
                Creature Mom = creatures[findNotDead()];
                Creature Dad = creatures[findNotDead()];
                Move[] mvsMom = Mom.WatRMyLegs();
                Move[] mvsDad = Dad.WatRMyLegs();
                Move[] myMvs = new Move[mvsMom.Length];
                for (int j = 0; j < myMvs.Length; j++)
                {
                    float rando = Random.value;
                    if (rando < 0.01f)
                    {
                        myMvs[j] = new Move();
                        //print("Mutation! " + i + " has gotten a random move!");
                    }
                    else if (rando > 0.1f && rando < 0.2f)
                    {
                        myMvs[j] = Move.RandAvg(mvsMom[j], mvsDad[j]);
                    }
                    else
                    {
                        myMvs[j] = Move.Avg(mvsMom[j], mvsDad[j]);
                    }
                    if (rando < 0.3f && rando > 0.4f)
                    {
                        myMvs[j].RandomizeTime();
                        //print("Mutation! " + i + " has gotten a move's time adjusted!");
                    }
                    else if (rando < 0.6f && rando > 0.7f)
                    {
                        myMvs[j].RandomizeAngle();
                        //print("Mutation! " + i + " has gotten a move's angle adjusted!");
                    }
                }
                creatures[i].ChangeLegs(myMvs);
                creatures[i].gameObject.transform.localScale = (Mom.gameObject.transform.localScale + Dad.gameObject.transform.localScale) / 2;
                if (Random.value < 0.05f)
                {
                    creatures[i].gameObject.transform.localScale = creatures[i].gameObject.transform.localScale * (1.0f + ((Random.value - 0.5f) * 0.05f));
                }
            }
            //Reset
            creatures[i].Reset();
        }
        CloseCam.GetComponent<Camera_Toggle>().target = bestWombat;
    }

    int findNotDead()
    {
        int i = Mathf.RoundToInt(Random.value * (oldCreatures.Length-1));
        while (oldCreatures[i].dead || oldCreatures[i].survivalChance < Random.value)
        {
            i = Mathf.RoundToInt(Random.value * (oldCreatures.Length-1));
        }
        return i;
    }
}
