using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Phil_Leg : MonoBehaviour {
    private Leg me;
    public void Start()
    {
        me = new Leg();
    }
}

class Leg
{
    int Upper_Length = 1;
    //int Lower_Length = 1;
   // int Foot_Size = 1;

    Quaternion Leg_Start_Rot = Quaternion.identity;
  //  Quaternion Knee_Start_Rot = Quaternion.identity;
  //  Quaternion Ankle_Start_Rot = Quaternion.identity;

    Quaternion Leg_Curr_Rot;
    //Quaternion Knee_Curr_Rot;
    //Quaternion Ankle_Curr_Rot;

    Move[] movements;
    int Cur_Move = 0;

    public Leg()
    {
        Leg_Curr_Rot = Leg_Start_Rot;
        // Knee_Curr_Rot = Knee_Start_Rot;
        // Ankle_Curr_Rot = Ankle_Start_Rot;
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


class Move
{
    Quaternion  ang;
    float       time;

    public Move(Quaternion _ang, float _t){
        ang = _ang;
        time = _t;
    }

    public static Move Avg(Move A,Move B)
    {
        return new Move(Quaternion.Slerp(A.ang, B.ang, 0.5f), (A.time + B.time) / 2);
    }

}
