using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Leg : MonoBehaviour {
    int Upper_Length = 1;
    //int Lower_Length = 1;
    // int Foot_Size = 1;

    Transform trans;

    Quaternion Leg_Start_Rot = Quaternion.identity;
  //  Quaternion Knee_Start_Rot = Quaternion.identity;
  //  Quaternion Ankle_Start_Rot = Quaternion.identity;

    Move[] movements;
    int Cur_Move = 0;
    float time_since_last_change;

    void Start()
    {
        trans = gameObject.GetComponent(typeof(Transform)) as Transform;
        time_since_last_change = Time.time;
    }

    public void SetMove(Move[] mv)
    {
        movements = mv;
    }

    public Move[] GetMove()
    {
        return movements;
    }

    void Update()
    {
        transform.rotation = Quaternion.Slerp(movements[Cur_Move].ang, movements[1-Cur_Move].ang, (Time.time - time_since_last_change)/movements[Cur_Move].time );
        if (trans.rotation == movements[1].ang)
        {
            Cur_Move = 1;
            time_since_last_change = Time.time;
        }
        else if (trans.rotation == movements[0].ang)
        {
            Cur_Move = 0;
            time_since_last_change = Time.time;
        }
    }

    public static Leg Avg(Leg A,Leg B)
    {
        Leg output = new Leg();
        output.Upper_Length = (A.Upper_Length + B.Upper_Length) / 2;
        output.Leg_Start_Rot = Quaternion.Slerp(A.Leg_Start_Rot, B.Leg_Start_Rot, 0.5f);
        int MoveCount = (A.movements.Length + B.movements.Length) / 2;
        output.movements = new Move[MoveCount];
        for (int i = 0; i < MoveCount; i++)
        {
            if(i<A.movements.Length && i< B.movements.Length)
            {
                output.movements[i] = Move.Avg(A.movements[i], B.movements[i]);
            }
            else
            {
                if (i < A.movements.Length)
                {
                    output.movements[i] = A.movements[i];
                }
                if (i < B.movements.Length)
                {
                    output.movements[i] = B.movements[i];
                }
            }
        }

        return output;
    }

    public void Mutate()
    {
        //TODO: See If we will actually need this...
    }

}